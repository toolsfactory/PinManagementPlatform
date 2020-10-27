using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PinPlatform.Common.Implementrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Domain.Infrastructure.DB;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Tiveria.Common.Extensions;
using PinPlatform.Domain.Models;

namespace PinPlatform.Services.AdminAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DBMaintenanceController : ControllerBase
    {
        private readonly ILogger<DBMaintenanceController> _logger;
        private readonly PinPlatform.Domain.Infrastructure.DB.DEMODBContext _context;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IPinCacheKeyGenerator _keyGen;

        public DBMaintenanceController(ILogger<DBMaintenanceController> logger, DEMODBContext context, IRedisCacheClient cacheClient, IPinCacheKeyGenerator keyGen)
        {
            _logger = logger;
            _context = context;
            _cacheClient = cacheClient;
            _keyGen = keyGen;
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
                var count = await CreateRecordsAsync(records);

                return Ok(new { Count = count });
            }
            return this.BadRequest();
        }


        [HttpGet]
        [Route("flushcache")]
        public async Task<IActionResult> FlushCacheAsync()
        {
            await _cacheClient.Db0.FlushDbAsync();
            return Ok();
        }

        [HttpGet]
        [Route("deelteallkeysfromcache")]
        public async Task<IActionResult> DeleteAllKeysFromCacheAsync([FromQuery] int maxrecords)
        {
            var keypattern = _keyGen.GenerateKeyBase("vfde") + "*";
            var items = await _cacheClient.Db0.SearchKeysAsync(keypattern);
            var cnt = await _cacheClient.Db0.RemoveAllAsync(items);
            return Ok(new { Deleted = cnt });
        }


        [HttpGet]
        [Route("populatecache")]
        public async Task<IActionResult> PopulateCacheAsync([FromQuery] int maxrecords)
        {
            var items = await _context.Pins.AsNoTracking().Take(maxrecords).ToListAsync();
            foreach(var item in items)
            {
                var model = new PinModel()
                {
                    PinHash = item.PinHash,
                    PinSalt = item.PinSalt,
                    PinLocked = item.PinLocked,
                    FailedAttemptsCount = 0,
                    LastFailedAttempt = DateTime.MinValue
                };
                var key = _keyGen.GenerateKeyForPin("vfde", item.HouseholdId, item.ProfileId, item.PinType);
                await _cacheClient.Db0.AddAsync(key, item);
            }
            return this.Ok();
        }

        private async Task<int> CreateRecordsAsync(int records)
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
                    OpcoId = "vfde",
                    HouseholdId = Guid.NewGuid().ToString(),
                    ProfileId = (uint) rand.Next(0, 5),
                    PinType = (uint) rand.Next(0, 2),
                    PinLocked = (rand.Next(0, 99) > 50),
                    PinSalt = salt,
                    Comments = pin,
                    PinHash = sha.ComputeHash(Encoding.ASCII.GetBytes(pin + salt)).ToHexString()
                };
            }
            await _context.AddRangeAsync(pins);
            return await _context.SaveChangesAsync();
        }
    }
}
