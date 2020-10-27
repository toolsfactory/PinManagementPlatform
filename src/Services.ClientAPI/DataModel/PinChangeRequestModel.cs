using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinPlatform.Services.ClientAPI.DataModel
{
    public class PinChangeRequestModel
    {
        [Required]
        public RequestorModel? Requestor { get; set; }
        [Range(0, 99)]
        public uint PinType { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPin { get; set; } = String.Empty;
        public string OldPinHash { get; set; } = String.Empty;
    }
}
