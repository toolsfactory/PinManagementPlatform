using System;
using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Services.ClientApi.DataModel
{
    public class PinHashModel
    {
        [Range(0, 99)]
        public uint PinType { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string PinHash { get; set; } = String.Empty;
    }
}
