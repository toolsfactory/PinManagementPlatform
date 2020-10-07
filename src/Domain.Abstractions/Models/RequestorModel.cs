namespace PinPlatform.Domain.Models
{
    public class RequestorModel
    {
        public string HouseholdId { get; set; } = string.Empty;
        public uint ProfileId { get; set; }
        public string OpCoId { get; set; } = string.Empty;
        public uint? PinType { get; set; }

    }

}