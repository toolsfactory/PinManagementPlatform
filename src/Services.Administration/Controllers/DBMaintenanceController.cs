using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Implementrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Services.Administration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBMaintenanceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<DBMaintenanceController> _logger;
        private readonly DEMODBContext _context;
        private readonly IRedisCacheClient _cacheClient;

        public DBMaintenanceController(ILogger<DBMaintenanceController> logger, DEMODBContext context, IRedisCacheClient cacheClient)
        {
            _logger = logger;
            _context = context;
            _cacheClient = cacheClient;
        }

        [HttpGet]
        [Route("status")]
        public IActionResult GetStatus()
        {
            var available = _context.Database.CanConnect();
            int pins = 0;
            if (available)
            {
                var results = _context.Database.ExecuteSqlRaw("SHOW TABLE STATUS FROM `DEMODB`");
                Console.WriteLine(results);
            }
            return Ok(new { Available = available, Count = pins });
        }

        [HttpGet]
        [Route("dropdb")]
        public IActionResult DropDB()
        {
            var available = _context.Database.CanConnect();
             if (available)
            {
                _context.Database.EnsureDeleted();
                return Ok();
            }
            return Ok();
        }

        [HttpGet]
        [Route("initdb")]
        public IActionResult InitDB()
        {
            var available = _context.Database.CanConnect();
            if (!available)
            {
                _context.Database.EnsureCreated();

                return Ok();
            }
            return this.BadRequest();
        }

        [HttpGet]
        [Route("createpins")]
        public async Task<IActionResult> CreatePinsAsync([FromQuery] int records)
        {
            var available = _context.Database.CanConnect();
            if (available)
            {
                await CreateRecordsAsync(records);

                return Ok();
            }
            return this.BadRequest();
        }

        [HttpGet]
        [Route("populatecache")]
        public async Task<IActionResult> PopulateCacheAsync([FromQuery] int maxrecords)
        {
            await _cacheClient.Db0.FlushDbAsync();
            var items = await _context.Pins.AsNoTracking().Take(maxrecords).ToListAsync();
            foreach(var item in items)
            {
                var sb = new StringBuilder("vfde");
                sb.Append("-");
                sb.Append(item.HouseholdId);
                sb.Append("-");
                sb.Append(item.ProfileId);
                sb.Append("-");
                sb.Append("pin-");
                sb.Append(item.PinType);
                sb.Append("-");
                sb.Append("hash");
                var key = sb.ToString();
                await _cacheClient.Db0.AddAsync(key, item.PinHash);
            }
            return this.Ok();
        }

        private async Task CreateRecordsAsync(int records)
        {
            var pins = new Pins[records];
            var rand = new Random();
            var sha = SHA256.Create();
            string pin = "";
            string salt = "";
            for(var i = 0; i<records; i++)
            {
                pin = "000" + rand.Next(0, 99999);
                pin = pin.Substring(pin.Length - 4);
                salt = rand.NextString(32);
                pins[i] = new Pins()
                {
                    HouseholdId = Guid.NewGuid().ToString(),
                    ProfileId = rand.Next(0, 5),
                    PinType = rand.Next(0, 3),
                    PinLocked = (rand.Next(0, 99) > 50),
                    PinSalt = salt,
                    Comments = pin,
                    PinHash = sha.ComputeHash(Encoding.ASCII.GetBytes(pin + salt)).ToHexString()
                };
            }
            await _context.AddRangeAsync(pins);
            await _context.SaveChangesAsync();
        }
    }
}
