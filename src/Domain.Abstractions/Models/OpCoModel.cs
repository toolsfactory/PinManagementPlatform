using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PinPlatform.Domain.Models
{
    public class OpCoModel
    {
        public OpCoModel(string id, IDictionary<ushort, PinTypeModel> pinTypes, ushort defaultType = 0)
        {
            PinTypes = pinTypes ?? throw new ArgumentNullException(nameof(pinTypes));
            Id = id;
            DefaultType = (ushort)(defaultType < pinTypes.Count ? defaultType : 0);
        }

        public string Id { get; set; } = String.Empty;
        public bool HasPins => PinTypes.Count > 0;
        public ushort DefaultType { get; set; }
        public IDictionary<ushort, PinTypeModel> PinTypes { get; set; } = new Dictionary<ushort, PinTypeModel>();

        public static OpCoModel FromOpCoConfig(Common.Configuration.OpCoConfig config)
        {
            var pintypes = new Dictionary<ushort, PinTypeModel>();
            if (config.PinTypes != null && config.PinTypes.Count > 0)
            {
                foreach (var ptype in config.PinTypes.OrderBy(x => x.Id))
                {
                    pintypes.Add(ptype.Id, PinTypeModel.FromPinTypeConfig(ptype));
                }
            }
            return new OpCoModel(config.Id, pintypes, config.DefaultType);
        }
    }
}
