namespace PinPlatform.Domain.Processors
{
    public class ChangePinParameters
    {
        public Models.RequestorModel Requestor { get; set; }
        public string OldPinHash { get; set; } = string.Empty;
        public uint PinType { get; set; } = 0;
        public string NewPin { get; set; } = string.Empty;
    }
}
