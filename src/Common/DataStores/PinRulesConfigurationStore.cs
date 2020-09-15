using Microsoft.Extensions.Hosting.Initialization;
using Microsoft.Extensions.Logging;
using PinPlatform.Common.DataModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PinPlatform.Common.Interfaces;

namespace PinPlatform.Common.DataStores
{

    public class PinRulesConfigurationStore : IRulesConfiguratonStore, IAsyncInitializer
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
            var opco1 = new OpCoConfiguration("vfde", pintypes, 0);
            var opco2 = new OpCoConfiguration("vfuk", new Dictionary<ushort, PinTypeDefinition>(), 0);
            _opCoConfigurations.Add(opco1.Id, opco1);
            _opCoConfigurations.Add(opco2.Id, opco2);

            var sw = Stopwatch.StartNew();
            var sha=SHA256.Create();
            for(var i = 0; i<1000000; i++)
            {
                var str = i.ToString();
                var bytes = Encoding.ASCII.GetBytes(str);
                var shaed = sha.ComputeHash(bytes);
            }
            sw.Stop();

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
