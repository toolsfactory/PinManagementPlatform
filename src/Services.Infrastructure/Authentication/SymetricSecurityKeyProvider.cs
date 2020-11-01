using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Text;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public class SymetricSecurityKeyProvider : ISecurityKeyProvider
    {
        public bool IsSymetric { get; }

        public IDictionary<string, SecurityKey> Keys { get; }

        public SymetricSecurityKeyProvider(IConfiguration config)
        {
            var keys = new Dictionary<string, SecurityKey>();
            var entries = config.GetSection(AuthenticationConsts.ConfigKeySymetricKeys).Get<Dictionary<string, string>>();
            foreach (var item in entries)
            {
                keys.Add(item.Key, new SymmetricSecurityKey(Encoding.UTF8.GetBytes(item.Value)));
            }
            Keys = keys;
            IsSymetric = true;
        }
    }
}
