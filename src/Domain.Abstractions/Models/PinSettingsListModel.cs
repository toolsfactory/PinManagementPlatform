using System.Collections.Generic;

namespace PinPlatform.Domain.Models
{
    public class PinSettingsListModel
    {
        private Dictionary<ushort, PinTypeDefinition> _pinTypes;

        public PinSettingsListModel(Dictionary<ushort, PinTypeDefinition> pinTypes)
        {
            _pinTypes = pinTypes;
        }

        public IReadOnlyDictionary<ushort, PinTypeDefinition> PinTypes => _pinTypes;
    }

}