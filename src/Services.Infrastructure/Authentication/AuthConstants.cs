namespace PinPlatform.Services.Infrastructure.Authentication
{
    public static class AuthConstants
    {
        public const string DefaultTokenHeader = "X-Internal-Auth";
        public const string DefaultValidIssuer = "MGTest-Issuer";
        public const string DefaultValidAudience = "MGTest-ValidAudience";

        public const string ConfigKeyHeaderName = "Jwt:Token:HeaderName";
        public const string ConfigKeyValidIssuer = "Jwt:Token:Issuer";
        public const string ConfigKeyValidAudience = "Jwt:Token:Audience";

        public const string ConfigKeyAsymetricPublicKeys = "Jwt:Asymmetric:PublicKeys";
        public const string ConfigKeyAsymetricPrivateKeys = "Jwt:Asymmetric:PrivateKeys";
        public const string ConfigKeySymetricKeys = "Jwt:Symmetric:Keys";
    }
}
