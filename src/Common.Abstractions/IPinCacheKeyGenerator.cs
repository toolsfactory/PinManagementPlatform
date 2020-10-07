namespace PinPlatform.Common
{
    public interface IPinCacheKeyGenerator
    {
        string GenerateKeyBase(string opcoId);
        string GenerateKeyForPin(string opcoid, string householdid, uint profileid, uint? pinType);
    }
}
