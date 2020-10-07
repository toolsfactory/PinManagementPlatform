using System;
using System.Collections.Generic;

namespace PinPlatform.Domain.Models
{
    public class PinTypeDefinition
    {
        private Dictionary<ushort,PinValidationRule> _validationRules = new Dictionary<ushort, PinValidationRule>();

        public PinTypeDefinition(Dictionary<ushort, PinValidationRule> validationRules, ushort id, string name, ushort minLength, ushort maxLength)
        {
            _validationRules = validationRules;
            Id = id;
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public ushort Id { get; private set; }
        public string Name { get; private set; } = String.Empty;
        public ushort MinLength { get; private set; }
        public ushort MaxLength { get; private set; }
        public IReadOnlyDictionary<ushort, PinValidationRule> ValidationRules { get => _validationRules; }
    }
}
