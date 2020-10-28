using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Models;
using System;
using System.Threading.Tasks;
using PinPlatform.Domain.Repositories;
using StackExchange.Redis.Extensions.Core.Abstractions;
using PinPlatform.Domain.Infrastructure.DB;
using PinPlatform.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PinPlatform.Domain.Infrastructure
{
    public class PinRepository : IPinRepository
    {
        private readonly ILogger<IPinRepository> _logger;
        private readonly IRedisCacheClient _redisClient;
        private readonly IPinCacheKeyGenerator _cacheKeyGenerator;
        private readonly DEMODBContext _dbContext;

        public PinRepository(ILogger<IPinRepository> logger, IRedisCacheClient redisClient, IPinCacheKeyGenerator cacheKeyGenerator, DEMODBContext dbContext)
        {
            _logger = logger;
            _redisClient = redisClient;
            _cacheKeyGenerator = cacheKeyGenerator;
            _dbContext = dbContext;
        }

        public async Task CreateOrUpdatePinAsync(RequestorModel requestor, PinModel pin)
        {
            var isNew = false;
            if (!requestor.PinType.HasValue)
                requestor.PinType = 0;
            var found = await _dbContext.Pins.SingleOrDefaultAsync(x => x.HouseholdId == requestor.HouseholdId && x.ProfileId == x.ProfileId && x.PinType == requestor.PinType);
            if (found is null)
            {
                found = new Pins()
                {
                    OpcoId = requestor.OpCoId,
                    HouseholdId = requestor.HouseholdId,
                    ProfileId = requestor.ProfileId,
                    PinType = requestor.PinType.Value
                };
                isNew = true;
            }
            found.PinHash = pin.PinHash;
            found.PinLocked = pin.PinLocked;
            found.PinSalt = pin.PinSalt;
            found.LockReason = pin.LockReason;

            if (isNew)
                await _dbContext.Pins.AddAsync(found);

            await _dbContext.SaveChangesAsync();

            var key = _cacheKeyGenerator.GenerateKeyForPin(requestor.OpCoId, requestor.HouseholdId, requestor.ProfileId, requestor.PinType);
            _redisClient.Db0.Database.StringSet(key, SerializePinModel(pin));
        }

        public async Task DeletePinAsync(RequestorModel requestor)
        {
            if (!requestor.PinType.HasValue)
                requestor.PinType = 0;
            var key = _cacheKeyGenerator.GenerateKeyForPin(requestor.OpCoId, requestor.HouseholdId, requestor.ProfileId, requestor.PinType);
            await _redisClient.Db0.RemoveAsync(key);
            var dbitem = new Pins()
            {
                OpcoId = requestor.OpCoId,
                HouseholdId = requestor.HouseholdId, 
                ProfileId = requestor.ProfileId, 
                PinType = requestor.PinType.Value
            };
            _dbContext.Remove(dbitem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PinModel> TryGetPinDetailsAsync(RequestorModel requestor)
        {
            PinModel model = default;
            var key = _cacheKeyGenerator.GenerateKeyForPin(requestor.OpCoId, requestor.HouseholdId, requestor.ProfileId, requestor.PinType);

            var cache = _redisClient.Db0.Database.StringGet(key);
            if (cache.HasValue)
                model = DeSerializePinModel(cache.ToString());
            else
            {
                var loaded = await _dbContext.Pins.SingleOrDefaultAsync(x => x.OpcoId == requestor.OpCoId && x.HouseholdId == requestor.HouseholdId && x.ProfileId == requestor.ProfileId && x.PinType == requestor.PinType);
                if (loaded != null)
                {
                    model = new PinModel()
                    {
                        PinHash = loaded.PinHash,
                        PinLocked = loaded.PinLocked,
                        LockReason = loaded.LockReason,
                        PinSalt = loaded.PinSalt,
                        FailedAttemptsCount = 0,
                        LastFailedAttempt = DateTime.MinValue
                    };
                    _redisClient.Db0.Database.StringSet(key, SerializePinModel(model));
                }
            }
            return model;
        }

        public async Task UpdatePinFailureInfoAsync(RequestorModel requestor, PinModel pin)
        {
            var key = _cacheKeyGenerator.GenerateKeyForPin(requestor.OpCoId, requestor.HouseholdId, requestor.ProfileId, requestor.PinType);
            await _redisClient.Db0.Database.StringSetAsync(key, SerializePinModel(pin));
        }

        private string SerializePinModel(PinModel pin)
        {
            return System.Text.Json.JsonSerializer.Serialize(pin);
        }

        private PinModel DeSerializePinModel(string raw)
        {
            return System.Text.Json.JsonSerializer.Deserialize<PinModel>(raw);
        }
    }
}
