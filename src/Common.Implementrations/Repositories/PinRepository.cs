using PinPlatform.Common.DataModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Text;
using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Tiveria.Common.Extensions;

namespace PinPlatform.Common.Repositories
{
    public class PinRepository : IPinRepository
    {
        private const string FailedLastSuffix = "-last";
        private const string FailedCountSuffix = "-count";

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

        #region IPinDataStore Implementation
        public async Task DeleteFailedAttemptsInfoAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = _cacheKeyGenerator.GenerateKeyForVerificationFailures(requestor, pinType);
            await _redisClient.Db0.RemoveAllAsync(new[] { prefix + FailedCountSuffix, prefix + FailedLastSuffix });
        }

        public async Task<(uint FailedAttemptsCount, System.DateTime LastFailedAttempt)> GetFailedVerificationsInfoAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = _cacheKeyGenerator.GenerateKeyForVerificationFailures(requestor, pinType);
            var FailedAttemptsCount = await _redisClient.Db0.GetAsync<uint?>(prefix + FailedCountSuffix) ?? 0;
            var LastFailedAttempt = await _redisClient.Db0.GetAsync<DateTime?>(prefix + FailedLastSuffix) ?? DateTime.MinValue;

            return (FailedAttemptsCount, LastFailedAttempt);
        }

        public async Task<string?> GetPinHashAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = _cacheKeyGenerator.GenerateKeyForHash(requestor, pinType);
            var hash = await _redisClient.Db0.GetAsync<string>(prefix);
            if (hash == null)
            {
                var loaded = await _dbContext.Pins.FirstOrDefaultAsync(x => x.HouseholdId == requestor.HouseholdId && x.ProfileId == requestor.ProfileId && x.PinType == pinType);
                if (loaded != null)
                {
                    await _redisClient.Db0.AddAsync(prefix, loaded.PinHash);
                    return loaded.PinHash;
                }
            }
            return hash;
        }

        public async Task SetPinAsync(RequestorInfo requestor, uint? pinType, string pin)
        {
            var sha = SHA256.Create();
            var loaded = await _dbContext.Pins.FirstOrDefaultAsync(x => x.HouseholdId == requestor.HouseholdId && x.ProfileId == requestor.ProfileId && x.PinType == pinType);
            loaded.PinHash = sha.ComputeHash(Encoding.ASCII.GetBytes(loaded.PinSalt + pin)).ToHexString();
            await _dbContext.SaveChangesAsync();
            var prefix = _cacheKeyGenerator.GenerateKeyForHash(requestor, pinType);
            await _redisClient.Db0.AddAsync(prefix, loaded.PinHash);
        }

        public async Task UpdateFailedVerificationsInfoAsync(RequestorInfo requestor, uint? pinType, uint failedAttempts, DateTime lastFailed)
        {
            var prefix = _cacheKeyGenerator.GenerateKeyForVerificationFailures(requestor, pinType);
            await _redisClient.Db0.AddAsync(prefix + FailedCountSuffix, failedAttempts);
            await _redisClient.Db0.AddAsync(prefix + FailedLastSuffix, lastFailed);
        }
        #endregion
    }
}
