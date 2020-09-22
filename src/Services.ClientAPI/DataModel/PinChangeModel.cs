using System;
using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Services.ClientApi.DataModel
{
    public class PinChangeModel
    {
        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string NewPin { get; set; } = String.Empty;
    }

}
