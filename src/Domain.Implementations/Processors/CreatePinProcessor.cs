using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public class CreatePinProcessor : ICreatePinProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IPinRepository _pinRepository;
        private readonly IPinChangeVerifier _pinChangeVerifier;
        private readonly IPinHashGenerator _pinHashGenerator;
        private readonly ILogger<VerifyPinProcessor> _logger;

        public CreatePinProcessor(IOpCoVerifier opcoVerifier, IPinRepository pinRepository, IPinChangeVerifier pinChangeVerifier, IPinHashGenerator pinHashGenerator, ILogger<VerifyPinProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _pinRepository = pinRepository;
            _pinChangeVerifier = pinChangeVerifier;
            _pinHashGenerator = pinHashGenerator;
            _logger = logger;
        }

        public async Task ProcessRequestAsync(RequestorModel requestor, string newPin)
        {
            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(requestor.OpCoId);
            var model = new PinChangeVerificationModel()
            {
                OpCoId = requestor.OpCoId,
                PinType = requestor.PinType,
                NewPin = newPin
            };
            await _pinChangeVerifier.CheckNewPinAgainstRulesAsync(model);

            var newpin = _pinHashGenerator.GeneratePinHashWithNewSalt(newPin);
            var pinmodel = new PinModel()
            {
                PinLocked = false,
                PinHash = newpin.Hash,
                PinSalt = newpin.Salt
            };

            await _pinRepository.CreateOrUpdatePinAsync(requestor, pinmodel);
        }
    }
}
