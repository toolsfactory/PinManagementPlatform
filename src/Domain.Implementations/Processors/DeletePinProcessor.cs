using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public class DeletePinProcessor : IDeletePinProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IPinRepository _pinRepository;
        private readonly ILogger<VerifyPinProcessor> _logger;

        public DeletePinProcessor(IOpCoVerifier opcoVerifier, IPinRepository pinRepository, ILogger<VerifyPinProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _pinRepository = pinRepository;
            _logger = logger;
        }

        public async Task ProcessRequestAsync(RequestorModel requestor)
        {
            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(requestor.OpCoId);
            await _pinRepository.DeletePinAsync(requestor);
        }
    }
}
