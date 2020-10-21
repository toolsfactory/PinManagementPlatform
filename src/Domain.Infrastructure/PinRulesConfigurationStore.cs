using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Initialization;
using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using PinPlatform.Common.Configuration;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Repositories
{
    public class PinRulesConfigurationStore : IRulesConfiguratonStore, IAsyncInitializer
    {
        private readonly ILogger<PinRulesConfigurationStore> _logger;
        private readonly IConfiguration _config;

        public PinRulesConfigurationStore(ILogger<PinRulesConfigurationStore> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private readonly Dictionary<string, OpCoModel> _opCoConfigurations = new Dictionary<string, OpCoModel>();
        public IDictionary<string, OpCoModel> OpCoConfigurations => _opCoConfigurations;


        public Task InitializeAsync()
        {
            var opcoOptions = _config.GetSection("opcos")
                                                     .Get<Common.Configuration.OpCosList>();

            foreach(var opco in opcoOptions.Opcos)
            {
                var newop = OpCoModel.FromOpCoConfig(opco);
                _opCoConfigurations.Add(newop.Id, newop);
            }

            return Task.CompletedTask;
        }
    }
}
