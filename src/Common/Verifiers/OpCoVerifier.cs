using Microsoft.Extensions.Logging;
using System;
using PinPlatform.Common.Interfaces;
using PinPlatform.Common.DataModels;

namespace PinPlatform.Common.Verifiers
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

        public (bool Success, ErrorCodes Error) CheckIfOpCoHasPinService(string opcoId)
        {
            if (string.IsNullOrEmpty(opcoId))
                throw new ArgumentNullException(nameof(opcoId));

            var lower = opcoId.ToLower();
            var found = _configStore.OpCoConfigurations.TryGetValue(lower, out OpCoConfiguration? op);

            if(!found || op is null)
                return (false, ErrorCodes.UnknownOpCo);
            
            if (!op.HasPins)
                return (false, ErrorCodes.OpCoWithoutPins);

            return (true, ErrorCodes.NoError);
        }
    }
}
