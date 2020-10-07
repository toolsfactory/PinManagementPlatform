using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Verifiers;
using System;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public class VerifyPinProcessor : IVerifyPinProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IPinHashVerifier _pinHashVerifier;
        private readonly ILogger<VerifyPinProcessor> _logger;

        public VerifyPinProcessor(IOpCoVerifier opcoVerifier, IPinHashVerifier pinHashVerifier, ILogger<VerifyPinProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _pinHashVerifier = pinHashVerifier;
            _logger = logger;
        }
        public async Task ProcessRequestAsync(VerifyPinParameters data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));
            if (data.Requestor is null)
                throw new ArgumentNullException(nameof(data.Requestor));

            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(data.Requestor.OpCoId);
            await _pinHashVerifier.VerifyPinHashAsync(data);
        }
    }
}
