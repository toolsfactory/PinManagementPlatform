namespace PinPlatform.Domain.Processors
{
    public class VerifyPinParameters
    {
        public Models.RequestorModel Requestor { get; set; }
        public string PinHash { get; set; } = string.Empty;
    }

}