using PinPlatform.Common;
using PinPlatform.Common.DataModels;
using System;
using System.Text;

namespace Common.Implementrations
{
    public class PinCacheKeyGenerator : IPinCacheKeyGenerator
    {
        public string GenerateKeyBase(string opcoId)
        {
            return $"{opcoId}-PIN_";
        }

        public string GenerateKeyForHash(RequestorInfo requestor, uint? pinType)
        {
            pinType = pinType ?? 0;
            return $"{requestor.OpCoId}-PIN_HASH-{requestor.HouseholdId}-{requestor.ProfileId}";
        }

        public string GenerateKeyForVerificationFailures(RequestorInfo requestor, uint? pinType)
        {
            pinType = pinType ?? 0;
            return $"{requestor.OpCoId}-PIN_VERIFY-{requestor.HouseholdId}-{requestor.ProfileId}";
        }
    }
}
