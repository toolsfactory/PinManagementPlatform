using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Services.ClientAPI.DataModel
{
    public class RequestorModel
    {
        [Required]
        public string HouseholdId { get; set; } = string.Empty;
        public uint ProfileId { get; set; }
    }
}
