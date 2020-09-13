﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PinPlatform.Services.PinChange.DataModel
{
    public class PinChangeRequestModel
    {
        [Required]
        public PinPlatform.Common.DataModels.RequestorInfo Requestor { get; set; }
        [Range(0,99)]
        public uint? PinType { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string OldPinHash { get; set; } = String.Empty;
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPin { get; set; } = String.Empty;
    }
}
