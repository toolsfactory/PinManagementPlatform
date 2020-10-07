using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PinPlatform.Domain.Models
{
    public class OpCoConfiguration
    {
        private readonly Dictionary<ushort, PinTypeDefinition> _pinTypes;

        public OpCoConfiguration(string id, Dictionary<ushort, PinTypeDefinition> pinTypes, ushort defaultType = 0)
        {
            _pinTypes = pinTypes ?? throw new ArgumentNullException(nameof(pinTypes));
            Id = id;
            DefaultType = (ushort)(defaultType < pinTypes.Count ? defaultType : 0);
        }

        public string Id { get; private set; } = String.Empty;
        public bool HasPins => _pinTypes.Count > 0;
        public ushort DefaultType { get; private set; }
        public IReadOnlyDictionary<ushort, PinTypeDefinition> PinTypes => _pinTypes;
    }
}
