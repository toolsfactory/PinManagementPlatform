using Microsoft.Extensions.Hosting.Initialization;
using Microsoft.Extensions.Logging;
using PinPlatform.Services.PinChange.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.PinChange
{
    public interface IRulesConfiguratonStore : IAsyncInitializer
    {
        IDictionary<string, OpCoConfiguration> OpCoConfigurations { get; }
    }

    public class PinRulesConfigurationStore : IRulesConfiguratonStore
    {
        private readonly ILogger<PinRulesConfigurationStore> _logger;

        public PinRulesConfigurationStore(ILogger<PinRulesConfigurationStore> logger)
        {
            _logger = logger;
        }

        private readonly Dictionary<string, OpCoConfiguration> _opCoConfigurations = new Dictionary<string, OpCoConfiguration>();
        public IDictionary<string, OpCoConfiguration> OpCoConfigurations => _opCoConfigurations;

        public Task InitializeAsync()
        {
            var pintypes = new Dictionary<ushort, PinTypeDefinition>();
            var t1 = GenerateHHPinDefinition();
            var t2 = GenerateParentalPinDefinition();
            pintypes.Add(t1.Id, t1);
            pintypes.Add(t2.Id, t2);
            var opco = new OpCoConfiguration("DE", pintypes, 0);

            return Task.CompletedTask;
        }

        private PinTypeDefinition GenerateHHPinDefinition()
        {
            var validationRules = new Dictionary<ushort, PinValidationRule>();
            validationRules.Add(3, new PinValidationRule(1, 3, PinValidationMode.ServerWithClientFallback, 30));
            validationRules.Add(6, new PinValidationRule(4, 6, PinValidationMode.ServerOnly, 60));
            validationRules.Add(ushort.MaxValue, new PinValidationRule(7, ushort.MaxValue, PinValidationMode.ServerOnly, 300));
            return new PinTypeDefinition(validationRules, 0, "Household Pin", 4, 6);
        }

        private PinTypeDefinition GenerateParentalPinDefinition()
        {
            var validationRules = new Dictionary<ushort, PinValidationRule>();
            validationRules.Add(3, new PinValidationRule(1, 3, PinValidationMode.ServerWithClientFallback, 5));
            validationRules.Add(6, new PinValidationRule(4, 6, PinValidationMode.ServerOnly, 10));
            validationRules.Add(ushort.MaxValue, new PinValidationRule(7, ushort.MaxValue, PinValidationMode.ServerOnly, 15));
            return new PinTypeDefinition(validationRules, 1, "Household Pin", 4, 4);
        }
    }
}
