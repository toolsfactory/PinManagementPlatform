namespace PinPlatform.Common
{
    public interface IPinCacheKeyGenerator
    {
        string GenerateKeyForHash(DataModels.RequestorInfo requestor, uint? pinType);
        string GenerateKeyForVerificationFailures(DataModels.RequestorInfo requestor, uint? pinType);
    }
}
