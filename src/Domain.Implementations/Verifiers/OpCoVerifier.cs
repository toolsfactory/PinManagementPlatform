using Microsoft.Extensions.Logging;
using System;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Verifiers
{
    public class OpCoVerifier : IOpCoVerifier
    {
        private readonly ILogger<OpCoVerifier> _logger;
        private readonly IRulesConfiguratonStore _configStore;

        public OpCoVerifier(ILogger<OpCoVerifier> logger, IRulesConfiguratonStore configStore)
        {
            _logger = logger;
            _configStore = configStore;
        }

        public Task CheckIfOpCoHasPinServiceAsync(string opcoId)
        {
            if (string.IsNullOrEmpty(opcoId))
                throw new ArgumentNullException(nameof(opcoId));

            var lower = opcoId.ToLower();
            var found = _configStore.OpCoConfigurations.TryGetValue(lower, out OpCoConfiguration? op);

            if (!found || op is null)
                throw new Exceptions.OpCoUnknownException();

            if (!op.HasPins)
                throw new Exceptions.OpCoNotSupportingPinsException();

            return Task.CompletedTask;
        }
    }
}
