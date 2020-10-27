using System.Collections.Generic;
using System.Security.Claims;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public interface IJwtTokenGenerator
    {
        (string Token, long ExpiresAt) GenerateToken(IEnumerable<Claim> claims, string keyId, ushort expiresSec = 90);
    }
}