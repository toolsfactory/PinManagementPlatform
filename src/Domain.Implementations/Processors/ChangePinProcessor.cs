using Microsoft.Extensions.Logging;
using PinPlatform.Common;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using System;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public class ChangePinProcessor : IChangePinProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IPinHashVerifier _pinHashVerifier;
        private readonly IPinChangeVerifier _pinChangeVerifier;
        private readonly IPinRepository _pinRepository;
        private readonly IPinHashGenerator _pinHashGenerator;
        private readonly ILogger<VerifyPinProcessor> _logger;

        public ChangePinProcessor(IOpCoVerifier opcoVerifier, IPinHashVerifier pinHashVerifier, IPinChangeVerifier pinChangeVerifier, IPinRepository pinRepository, IPinHashGenerator pinHashGenerator, ILogger<VerifyPinProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _pinHashVerifier = pinHashVerifier;
            _pinChangeVerifier = pinChangeVerifier;
            _pinRepository = pinRepository;
            _pinHashGenerator = pinHashGenerator;
            _logger = logger;
        }

        public async Task ProcessRequestAsync(ChangePinParameters data)
        {
            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(data.Requestor.OpCoId);
            var param = new VerifyPinParameters()
            {
                Requestor = data.Requestor,
                PinHash = data.OldPinHash

            };
            var pinmodel = await _pinHashVerifier.VerifyPinHashAsync(param);
            var model = new PinChangeVerificationModel()
            {
                OpCoId = data.Requestor.OpCoId,
                PinType = data.PinType,
                NewPin = data.NewPin
            };
            await _pinChangeVerifier.CheckNewPinAgainstRulesAsync(model);

            var newpin = _pinHashGenerator.GeneratePinHashWithNewSalt(data.NewPin);
            pinmodel.PinHash = newpin.Hash;
            pinmodel.PinSalt = newpin.Salt;

            await _pinRepository.CreateOrUpdatePinAsync(data.Requestor, pinmodel);
        }
    }
}
