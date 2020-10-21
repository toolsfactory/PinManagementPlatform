using Microsoft.Extensions.Logging;
using PinPlatform.Domain.Models;
using PinPlatform.Domain.Repositories;
using PinPlatform.Domain.Verifiers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinPlatform.Domain.Processors
{
    public class PinSettingsProcessor : IPinSettingsProcessor
    {
        private readonly IOpCoVerifier _opcoVerifier;
        private readonly IRulesConfiguratonStore _rulesConfiguratonStore;
        private readonly IPinRepository _pinRepository;
        private readonly ILogger<PinSettingsProcessor> _logger;

        public PinSettingsProcessor(IOpCoVerifier opcoVerifier, IRulesConfiguratonStore rulesConfiguratonStore, IPinRepository pinRepository, ILogger<PinSettingsProcessor> logger)
        {
            _opcoVerifier = opcoVerifier;
            _rulesConfiguratonStore = rulesConfiguratonStore;
            _pinRepository = pinRepository;
            _logger = logger;
        }

        public async Task<PinSettingsListModel> ProcessRequestAsync(RequestorModel data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            await _opcoVerifier.CheckIfOpCoHasPinServiceAsync(data.OpCoId);
            var config = _rulesConfiguratonStore.OpCoConfigurations[data.OpCoId];
            if (config is null)
                throw new InvalidOperationException("No config details for requested opco");

            var individual = new Dictionary<string, PinDetailsModel>();
            foreach (var item in config.PinTypes)
            {
                data.PinType = item.Value.Id;
                var pin = await _pinRepository.TryGetPinDetailsAsync(data);
                if (pin == null)
                    continue;
                var individualItem = new PinDetailsModel(item.Value, pin.PinSalt, pin.PinHash, pin.PinLocked);
                individual.Add(individualItem.Id.ToString(), individualItem);
            }

            return await Task.FromResult(new PinSettingsListModel(individual));
        }
    }
}
