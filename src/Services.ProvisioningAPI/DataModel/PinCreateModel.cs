using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinPlatform.Services.ProvisioningAPI.DataModel
{
    public class PinCreateModel
    {
        [Required]
        public string HouseholdId { get; set; } = string.Empty;
        public uint ProfileId { get; set; }

        [Range(0, 99)]
        public uint PinType { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPin { get; set; } = String.Empty;
    }

    public class PinChangeModel
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPin { get; set; } = String.Empty;
    }

    public class PinLockModel
    {
        public string Reason { get; set; } = String.Empty;
    }

}
