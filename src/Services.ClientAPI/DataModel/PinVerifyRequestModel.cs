using System;
using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Services.ClientApi.DataModel
{
    public class PinVerifyRequestModel
    {
        [Required]
        public RequestorModel? Requestor { get; set; }
        [Range(0,99)]
        public uint PinType { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string PinHash { get; set; } = String.Empty;
    }

}
