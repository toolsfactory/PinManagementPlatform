namespace PinPlatform.Common.Configuration
{
    public enum PinValidationMode : ushort
    {
        ClientOnly = 0,
        ServerWithClientFallback = 1,
        ServerOnly = 2
    }
}
