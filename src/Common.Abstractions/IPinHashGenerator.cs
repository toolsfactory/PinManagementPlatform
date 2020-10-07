namespace PinPlatform.Common
{
    public interface IPinHashGenerator
    {
        string GeneratePinHash(string pin, string salt);
        (string Hash, string Salt) GeneratePinHashWithNewSalt(string pin);
    }
}
