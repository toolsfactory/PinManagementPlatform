using System;
using System.Collections.Generic;

namespace PinPlatform.Domain.Infrastructure.DB
{
    public partial class Pins
    {
        public string OpcoId { get; set; }
        public string HouseholdId { get; set; }
        public uint ProfileId { get; set; }
        public uint PinType { get; set; }
        public string PinSalt { get; set; }
        public string PinHash { get; set; }
        public bool PinLocked { get; set; }
        public string Comments { get; set; }
    }
}
