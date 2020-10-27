using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public interface ISecurityKeyProvider
    {
        IDictionary<string, SecurityKey> Keys { get; }
        bool IsSymetric { get; }
    }
}
