using System.ComponentModel.DataAnnotations;

namespace PinPlatform.Common.DataModels
{
    public class RequestorInfo
    {
        [Required]
        public string HouseholdId { get; set; } = string.Empty;
        public string? ProfileId { get; set; }
        public string OpCoId { get; set; } = string.Empty;
    }
}