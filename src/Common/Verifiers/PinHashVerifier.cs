using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PinPlatform.Common.Interfaces;

namespace PinPlatform.Common.Verifiers
{
    public class PinHashVerifier : IPinHashVerifier
    {
        private const string FailedLastSuffix = "failed-last";
        private const string FailedCountSuffix = "failed-count";
        private const string HashSuffix = "hash";

        private readonly ILogger<PinHashVerifier> _logger;
        private readonly IRedisCacheClient _redisClient;

        private string _pinHash = String.Empty;
        private uint? _pinType;
        private string _cachePrefix = String.Empty;
        private string? _storedPinHash = default;
        private DataModels.RequestorInfo? _requestor;

        public int FailedAttemptsCount { get; private set; }
        public DateTime LastFailedAttempt { get; private set; }

        public PinHashVerifier(ILogger<PinHashVerifier> logger, IRedisCacheClient redisClient)
        {
            _logger = logger;
            _redisClient = redisClient;
        }

        public async Task<(bool Success, ErrorCodes Error)> VerifyPinHashAsync(Common.DataModels.RequestorInfo requestor, uint? pinType, string pinHash)
        {
            if (requestor is null)
                throw new ArgumentNullException(nameof(requestor));

            _requestor = requestor;
            _pinHash = pinHash;
            _pinType = pinType;

            GenerateCachingPrefix();
            await LoadFailedAttemptsInfoAsync();

            if (IsInGracePeriod())
            {
                return (false, ErrorCodes.WithinGracePeriod);
            }

            if (!await LoadPinHashAsync())
                return (false, ErrorCodes.NoPinHashFound);

            if (pinHash == _storedPinHash)
            {
                await RemoveFailedAttemptsInfoAsync();
                LogSuccessfulVerification();
                return (true, ErrorCodes.NoError);
            }
            else
            {
                await UpdateFailedAttemptsInfoAsync();
                LogFailedVerification();
                return (false, ErrorCodes.PinHashesNotMatching);
            }
        }

        private void LogFailedVerification()
        {
        }

        private void LogSuccessfulVerification()
        {
        }

        private async Task LoadFailedAttemptsInfoAsync()
        {
            FailedAttemptsCount = await _redisClient.Db0.GetAsync<int?>(_cachePrefix + FailedCountSuffix) ?? 0;
            LastFailedAttempt = await _redisClient.Db0.GetAsync<DateTime?>(_cachePrefix + FailedLastSuffix) ?? DateTime.MinValue;
        }

        private async Task RemoveFailedAttemptsInfoAsync()
        {
            await _redisClient.Db0.RemoveAllAsync(new[] { _cachePrefix + FailedCountSuffix, _cachePrefix + FailedLastSuffix });
        }

        private async Task UpdateFailedAttemptsInfoAsync()
        {
            FailedAttemptsCount++;
            LastFailedAttempt = DateTime.Now;
            await _redisClient.Db0.AddAsync(_cachePrefix + FailedCountSuffix, FailedAttemptsCount);
            await _redisClient.Db0.AddAsync(_cachePrefix + FailedLastSuffix, LastFailedAttempt);
        }

        private bool IsInGracePeriod()
        {
            if (FailedAttemptsCount == 0)
                return false;

            var gracePeriodInSec = GetGracePeriodForFailedCount(FailedAttemptsCount);

            return LastFailedAttempt switch
            {
                DateTime last when last < DateTime.Now.AddSeconds(-gracePeriodInSec) => false,
                DateTime last when last >= DateTime.Now.AddSeconds(-gracePeriodInSec) => true,
                _ => true
            };
        }

        private int GetGracePeriodForFailedCount(int failedAttemptsCount) => failedAttemptsCount switch
        {
            int count when count < 3 => 30,
            int count when count < 6 => 60,
            int count when count < 9 => 90,
            _ => 120
        };

        private async Task<bool> LoadPinHashAsync()
        {
            try
            {
                var result = await _redisClient.Db0.Database.StringGetAsync(_cachePrefix + HashSuffix);
                _storedPinHash = result.HasValue ? result.ToString() : default;
                return _storedPinHash != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GenerateCachingPrefix()
        {
            var sb = new StringBuilder(_requestor!.OpCoId);
            sb.Append("-");
            sb.Append(_requestor.HouseholdId);
            sb.Append("-");
            if (_requestor.ProfileId != null)
            {
                sb.Append(_requestor.ProfileId);
                sb.Append("-");
            }
            sb.Append("pin-");
            sb.Append(_pinType.HasValue ? _pinType.Value.ToString() : "D");
            sb.Append("-");
            _cachePrefix = sb.ToString();
        }

    }
}
