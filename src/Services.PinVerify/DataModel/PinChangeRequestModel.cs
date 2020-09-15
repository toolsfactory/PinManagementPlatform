using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinPlatform.Services.PinVerify.DataModel
{
    public class PinVerifyRequestModel
    {
        [Required]
        public PinPlatform.Common.DataModels.RequestorInfo? Requestor { get; set; }
        [Range(0,99)]
        public uint PinType { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string PinHash { get; set; } = String.Empty;
    }
}
