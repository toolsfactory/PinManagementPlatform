using PinPlatform.Domain.Models;
using PinPlatform.Domain.Processors;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;

namespace PinPatform.Domain.Processors
{
    public class LockPinProcessor : ILockPinProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IPinRepository _pinRepository;
        private readonly ILogger<LockPinProcessor> _logger;

        public LockPinProcessor(IOpCoVerifier opcoVerifier, IPinRepository pinRepository, ILogger<LockPinProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _pinRepository = pinRepository;
            _logger = logger;
        }
        public async Task ProcessRequestAsync(RequestorModel requestor, bool lockpin = true , string reason = null)
        {
            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(requestor.OpCoId);

            var pinModel = await _pinRepository.TryGetPinDetailsAsync(requestor);
            if (pinModel is null)
                throw new PinPlatform.Domain.Exceptions.PinDoesntExistException();

            pinModel.PinLocked = lockpin;
            pinModel.LockReason = lockpin ? reason : String.Empty;

            await _pinRepository.CreateOrUpdatePinAsync(requestor, pinModel);
        }
    }
}
