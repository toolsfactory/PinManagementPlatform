using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using System.Text;

namespace PinPlatform.Common.Implementrations
{
    public class PinCacheKeyGenerator : IPinCacheKeyGenerator
    {
        public string GenerateKeyBase(string opcoId)
        {
            return $"{opcoId}-PIN";
        }

        public string GenerateKeyForPin(string opcoId, string householdId, uint profileId, uint? pinType)
        {
            pinType ??= 0;
            return $"{opcoId}/PIN/{householdId}/{profileId}/{pinType.Value}";
        }
    }
}
