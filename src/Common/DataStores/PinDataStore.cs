using PinPlatform.Common.DataModels;
using System.Threading.Tasks;
using PinPlatform.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Text;
using System;
using Microsoft.Extensions.Hosting.Initialization;
using System.Security.Cryptography;

namespace PinPlatform.Common.DataStores
{
    public class PinDataStore : IPinDataStore, IAsyncInitializer
    {
        private const string FailedLastSuffix = "failed-last";
        private const string FailedCountSuffix = "failed-count";
        private const string HashSuffix = "hash";

        private readonly ILogger<IPinDataStore> _logger;
        private readonly IRedisCacheClient _redisClient;

        public PinDataStore(ILogger<IPinDataStore> logger, IRedisCacheClient redisClient)
        {
            _logger = logger;
            _redisClient = redisClient;
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

        public async Task<byte[]?> GetPinHashAsync(RequestorInfo requestor, uint? pinType)
        {
            var prefix = GenerateCachingPrefix(requestor, pinType);
            var hash = await _redisClient.Db0.GetAsync<byte[]?>(prefix + HashSuffix);
            //            var hash = await _redisClient.Db0.Database.StringGetAsync(prefix + HashSuffix);
            return hash;
        }

        public async Task SetPinHashAsync(RequestorInfo requestor, uint? pinType, byte[] hash)
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

        #region IAsyncInitializer implementation
        public async Task InitializeAsync()
        {
            var sha = SHA256.Create();
            var req = new RequestorInfo() { HouseholdId = "0000", OpCoId = "vfde", ProfileId = "1" };
            var prefix = GenerateCachingPrefix(req, 1);
            await _redisClient.Db0.AddAsync(prefix + FailedCountSuffix, 2);
            await _redisClient.Db0.AddAsync(prefix + FailedLastSuffix, DateTime.Now);
            await _redisClient.Db0.AddAsync(prefix + HashSuffix, sha.ComputeHash(Encoding.ASCII.GetBytes("1234")));

            req = new RequestorInfo() { HouseholdId = "0001", OpCoId = "vfde", ProfileId = "1" };
            prefix = GenerateCachingPrefix(req, 1);
            await _redisClient.Db0.AddAsync(prefix + HashSuffix, sha.ComputeHash(Encoding.ASCII.GetBytes("1234")));
        }
        #endregion

        #region private helpers
        private string GenerateCachingPrefix(RequestorInfo requestor, uint? pinType)
        {
            var sb = new StringBuilder(requestor!.OpCoId);
            sb.Append("-");
            sb.Append(requestor.HouseholdId);
            sb.Append("-");
            if (requestor.ProfileId != null)
            {
                sb.Append(requestor.ProfileId);
                sb.Append("-");
            }
            sb.Append("pin-");
            sb.Append(pinType.HasValue ? pinType.Value.ToString() : "D");
            sb.Append("-");
            return sb.ToString();
        }
        #endregion
    }
}
