﻿using Microsoft.Extensions.Logging;
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
        private readonly IPinDataStore _pinDataStore;

        private string _pinHash = String.Empty;
        private uint? _pinType;
        private string _cachePrefix = String.Empty;
        private string? _storedPinHash = default;
        private DataModels.RequestorInfo? _requestor;

        public uint FailedAttemptsCount { get; private set; }
        public DateTime LastFailedAttempt { get; private set; }

        public PinHashVerifier(ILogger<PinHashVerifier> logger, IPinDataStore pinDataStore)
        {
            _logger = logger;
            _pinDataStore = pinDataStore;
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
            (FailedAttemptsCount, LastFailedAttempt) = await _pinDataStore.GetFailedVerificationsInfoAsync(_requestor!, _pinType);
        }

        private async Task RemoveFailedAttemptsInfoAsync()
        {
            await _pinDataStore.DeleteFailedAttemptsInfoAsync(_requestor!, _pinType);
        }

        private async Task UpdateFailedAttemptsInfoAsync()
        {
            FailedAttemptsCount++;
            LastFailedAttempt = DateTime.Now;
            await _pinDataStore.UpdateFailedVerificationsInfoAsync(_requestor!, _pinType, FailedAttemptsCount, LastFailedAttempt);
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

        private int GetGracePeriodForFailedCount(uint failedAttemptsCount) => failedAttemptsCount switch
        {
            uint count when count < 3 => 30,
            uint count when count < 6 => 60,
            uint count when count < 9 => 90,
            _ => 120
        };

        private async Task<bool> LoadPinHashAsync()
        {
            try
            {
                _storedPinHash = await _pinDataStore.GetPinHashAsync(_requestor!, _pinType);
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
