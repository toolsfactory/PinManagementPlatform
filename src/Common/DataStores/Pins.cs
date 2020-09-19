using System;
using System.Collections.Generic;

namespace PinPlatform.Common
{
    public partial class Pins
    {
        public string HouseholdId { get; set; }
        public int ProfileId { get; set; }
        public int PinType { get; set; }
        public string PinSalt { get; set; }
        public string PinHash { get; set; }
        public byte PinLocked { get; set; }
        public string Comments { get; set; }
    }
}
