using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Models;

namespace PinPlatform.Domain.Verifiers
{
    public class PinHashVerifier : IPinHashVerifier
    {
        private readonly ILogger<PinHashVerifier> _logger;
        private readonly IPinRepository _pinDataStore;

        private PinModel _storedPinModel = default;
        private RequestorModel _requestor = default;

        public PinHashVerifier(ILogger<PinHashVerifier> logger, IPinRepository pinDataStore)
        {
            _logger = logger;
            _pinDataStore = pinDataStore;
        }

        public async Task<PinModel> VerifyPinHashAsync(Domain.Processors.VerifyPinParameters data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            if (data.Requestor is null)
                throw new ArgumentNullException(nameof(data.Requestor));
            _requestor = data.Requestor;

            await LoadPinModelAsync(data.Requestor);

            if (IsInGracePeriod(out int secsToWait))
                throw new Exceptions.PinVerificationWithinGracePeriodException(_storedPinModel.FailedAttemptsCount, secsToWait);

            if ((data.PinHash.Length != _storedPinModel.PinHash.Length) || !data.PinHash.Equals(_storedPinModel.PinHash))
            {
                await UpdateFailedAttemptsInfoAsync();
                LogFailedVerification();
                throw new Exceptions.PinInvalidException(_storedPinModel.FailedAttemptsCount, GetGracePeriodForFailedCount(_storedPinModel.FailedAttemptsCount));
            }

            await RemoveFailedAttemptsInfoAsync();
            LogSuccessfulVerification();
            return _storedPinModel;
        }

        private async Task LoadPinModelAsync(RequestorModel requestor)
        {
            _storedPinModel = await _pinDataStore.TryGetPinDetailsAsync(requestor);
            if (_storedPinModel is null)
                throw new Exceptions.PinDoesntExistException();
        }

        private bool IsInGracePeriod(out int secondsToWait)
        {
            secondsToWait = 0;
            if (_storedPinModel.FailedAttemptsCount == 0)
                return false;

            var gracePeriodInSec = GetGracePeriodForFailedCount(_storedPinModel.FailedAttemptsCount);
            var boundry = DateTime.Now.AddSeconds(-gracePeriodInSec);

            var inGrace =  _storedPinModel.LastFailedAttempt switch
            {
                DateTime last when last < boundry => false,
                DateTime last when last >= boundry => true,
                _ => true
            };

            if (inGrace)
                secondsToWait = (int) (_storedPinModel.LastFailedAttempt - boundry).TotalSeconds;

            return inGrace;
        }
        private int GetGracePeriodForFailedCount(uint failedAttemptsCount) => failedAttemptsCount switch
        {
            uint count when count < 3 => 30,
            uint count when count < 6 => 60,
            uint count when count < 9 => 90,
            _ => 120
        };


        private void LogFailedVerification()
        {
        }

        private void LogSuccessfulVerification()
        {
        }

        private async Task RemoveFailedAttemptsInfoAsync()
        {
            _storedPinModel.LastFailedAttempt = DateTime.MinValue;
            _storedPinModel.FailedAttemptsCount = 0;
            await _pinDataStore.UpdatePinFailureInfoAsync(_requestor, _storedPinModel);
        }

        private async Task UpdateFailedAttemptsInfoAsync()
        {
            _storedPinModel.LastFailedAttempt = DateTime.Now;
            _storedPinModel.FailedAttemptsCount++;
            await _pinDataStore.UpdatePinFailureInfoAsync(_requestor, _storedPinModel);
        }
    }
}
