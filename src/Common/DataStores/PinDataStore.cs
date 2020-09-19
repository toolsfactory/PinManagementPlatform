using PinPlatform.Common.DataModels;
using System.Threading.Tasks;
using PinPlatform.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Text;
using System;
using Microsoft.Extensions.Hosting.Initialization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace PinPlatform.Common.DataStores
{
    public class PinDataStore : IPinDataStore
    {
        private const string FailedLastSuffix = "failed-last";
        private const string FailedCountSuffix = "failed-count";
        private const string HashSuffix = "hash";

        private readonly ILogger<IPinDataStore> _logger;
        private readonly IRedisCacheClient _redisClient;
        private readonly DEMODBContext _dbContext;

        public PinDataStore(ILogger<IPinDataStore> logger, IRedisCacheClient redisClient, DEMODBContext dbContext)
        {
            _logger = logger;
            _redisClient = redisClient;
            _dbContext = dbContext;
        }

        #region IPinDataStore Implenetation
        public async Task DeleteFailedAttemptsInfoAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = GenerateCachingPrefix(requestor, pinType);
            await _redisClient.Db0.RemoveAllAsync(new[] { prefix + FailedCountSuffix, prefix + FailedLastSuffix });
        }

        public async Task<(uint FailedAttemptsCount, System.DateTime LastFailedAttempt)> GetFailedVerificationsInfoAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = GenerateCachingPrefix(requestor, pinType);
            var FailedAttemptsCount = await _redisClient.Db0.GetAsync<uint?>(prefix + FailedCountSuffix) ?? 0;
            var LastFailedAttempt = await _redisClient.Db0.GetAsync<DateTime?>(prefix + FailedLastSuffix) ?? DateTime.MinValue;

            return (FailedAttemptsCount, LastFailedAttempt);
        }

        public async Task<string?> GetPinHashAsync(RequestorInfo requestor, uint? pinType)
        {
            try
            {
                var prefix = GenerateCachingPrefix(requestor, pinType);
                var hash = await _redisClient.Db0.GetAsync<string>(prefix + HashSuffix);
                if (hash == null)
                {
                    var loaded = await _dbContext.Pins.FirstOrDefaultAsync(x => x.HouseholdId == requestor.HouseholdId && x.ProfileId == requestor.ProfileId && x.PinType == pinType);
                    if (loaded != null)
                    {
                        await _redisClient.Db0.AddAsync(prefix + HashSuffix, loaded.PinHash);
                        return loaded.PinHash;
                    }
                }
                return hash;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SetPinHashAsync(RequestorInfo requestor, uint? pinType, string hash)
        {
            var prefix = GenerateCachingPrefix(requestor, pinType);
            await _redisClient.Db0.AddAsync(prefix + HashSuffix, hash);
        }

        public async Task UpdateFailedVerificationsInfoAsync(RequestorInfo requestor, uint? pinType, uint failedAttempts, DateTime lastFailed)
        {
            var prefix = GenerateCachingPrefix(requestor, pinType);
            await _redisClient.Db0.AddAsync(prefix + FailedCountSuffix, failedAttempts);
            await _redisClient.Db0.AddAsync(prefix + FailedLastSuffix, lastFailed);
        }
        #endregion

        #region private helpers
        private string GenerateCachingPrefix(RequestorInfo requestor, uint? pinType)
        {
            var sb = new StringBuilder(requestor!.OpCoId);
            sb.Append("-");
            sb.Append(requestor.HouseholdId);
            sb.Append("-");
            sb.Append(requestor.ProfileId);
            sb.Append("-");
            sb.Append("pin-");
            sb.Append(pinType.HasValue ? pinType.Value.ToString() : "0");
            sb.Append("-");
            return sb.ToString();
        }
        #endregion
    }
}
