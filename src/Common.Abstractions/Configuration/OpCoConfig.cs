using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PinPlatform.Common.Configuration
{
    public class OpCosList
    {
        public IList<OpCoConfig>? Opcos { get; set; }
    }
    public class OpCoConfig
    {
        public string Id { get; set; } = String.Empty;
        public ushort DefaultType { get; set; }
        public IList<PinTypeConfig>? PinTypes { get; set; }
    }

    public class PinTypeConfig
    {
        public ushort Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ushort MinLength { get; set; }
        public ushort MaxLength { get; set; }
        public IList<PinValidationRuleConfig>? ValidationRules { get; set; }
    }

    public class PinValidationRuleConfig
    {
        public ushort MinErrors { get; set; }
        public ushort MaxErrors { get; set; }
        public PinValidationMode ValidationMode { get; set; }
        public ushort GracePeriodSec { get; set; }
    }

}
