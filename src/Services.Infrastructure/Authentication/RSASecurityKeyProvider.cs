using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public class RSASecurityKeyProvider : ISecurityKeyProvider
    {
        public IDictionary<string, SecurityKey> Keys { get; }

        public bool IsSymetric { get; }

        public RSASecurityKeyProvider(IConfiguration config)
        {
            var keys = new Dictionary<string, SecurityKey>();
            var entries = config.GetSection(AuthConstants.ConfigKeyAsymetricPublicKeys).Get<Dictionary<string, string>>();
            
            foreach (var item in entries)
            {
                RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(
                    source: Convert.FromBase64String(item.Value),
                    bytesRead: out int _
                );
                keys.Add(item.Key, new RsaSecurityKey(rsa));
            }
            Keys = keys;
            IsSymetric = false;
        }
    }
}
