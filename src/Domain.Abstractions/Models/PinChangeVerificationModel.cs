namespace PinPlatform.Domain.Models
{
    public class PinChangeVerificationModel
    {
        public string OpCoId { get; set; } = string.Empty;
        public uint? PinType { get; set; }
        public string NewPin { get; set; } = string.Empty;
    }
}