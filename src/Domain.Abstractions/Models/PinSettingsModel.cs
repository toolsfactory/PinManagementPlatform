using System.Collections.Generic;

namespace PinPlatform.Domain.Models
{
    public class PinSettingsModel
    {
        private Dictionary<ushort, PinValidationRule> _validationRules = new Dictionary<ushort, PinValidationRule>();

        public PinSettingsModel(uint pinType, string pinHash, string pinSalt, Dictionary<ushort, PinValidationRule> validationRules)
        {
            PinType = pinType;
            PinHash = pinHash;
            PinSalt = pinSalt;
            _validationRules = validationRules;
        }

        public string PinHash { get; private set; } = string.Empty;
        public string PinSalt { get; private set; } = string.Empty;
        public uint PinType { get; private set; } = 0;
        public IReadOnlyDictionary<ushort, PinValidationRule> ValidationRules { get => _validationRules; }
    }

}