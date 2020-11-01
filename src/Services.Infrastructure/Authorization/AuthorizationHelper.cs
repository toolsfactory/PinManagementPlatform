using System.Security.Claims;

namespace PinPlatform.Services.Infrastructure.Authorization
{
    public static class AuthorizationHelper
    {
        public const string ClientAccessPolicy = "ClientAccess";
        public const string AdminAccessPolicy = "AdminAccess";
        public const string ProvisioningAccessPolicy ="ProvisioningAccess";

        public static string GetClaimNameFor(string policyName) => policyName switch
        {
            ClientAccessPolicy => "x-a-client",
            AdminAccessPolicy => "x-a-admin",
            ProvisioningAccessPolicy => "x-a-provision",
            _ => null
        };

        public static Claim GetClaimFor(string policyName, string value = "true")
        {
            var name = GetClaimNameFor(policyName);
            if (name == null)
                return null;
            return new Claim(name, value);
        }
    }


}
