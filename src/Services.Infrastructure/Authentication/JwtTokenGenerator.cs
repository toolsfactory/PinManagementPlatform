using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PinPlatform.Services.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityKeyProvider _keyProvider;

        public JwtTokenGenerator(IConfiguration configuration, ISecurityKeyProvider keyProvider)
        {
            _configuration = configuration;
            _keyProvider = keyProvider;
        }

        public (string Token, long ExpiresAt) GenerateToken(IEnumerable<Claim> claims, string keyId, ushort expiresSec = 90)
        {
            var credentials = _keyProvider.IsSymetric ? GetSymetricCredentials(keyId) : GetAsymetricCredentials(keyId);
            var dtStart = DateTime.Now;
            var dtEnd = dtStart.AddSeconds(expiresSec);

            var jwt = new JwtSecurityToken(
                audience: _configuration.GetValue(AuthenticationConsts.ConfigKeyValidAudience, AuthenticationConsts.DefaultValidAudience), 
                issuer: _configuration.GetValue(AuthenticationConsts.ConfigKeyValidIssuer, AuthenticationConsts.DefaultValidIssuer), 
                claims: claims,
                notBefore: dtStart,
                expires: dtEnd,
                signingCredentials: credentials
            );
            jwt.Header.Add("kid", keyId);

            return (new JwtSecurityTokenHandler().WriteToken(jwt), new DateTimeOffset(dtEnd).ToUnixTimeSeconds()) ;
        }

        private SigningCredentials GetAsymetricCredentials(string keyId)
        {
            var entries = _configuration.GetSection(AuthenticationConsts.ConfigKeyAsymetricPrivateKeys).Get<Dictionary<string, string>>();
            if (!entries.ContainsKey(keyId))
                throw new ArgumentOutOfRangeException(nameof(keyId));

            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(entries[keyId]),  out int _);
            return new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        }

        private SigningCredentials GetSymetricCredentials(string keyId)
        {
            var entries = _configuration.GetSection(AuthenticationConsts.ConfigKeySymetricKeys).Get<Dictionary<string, string>>();
            if (!entries.ContainsKey(keyId))
                throw new ArgumentOutOfRangeException(nameof(keyId));

            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(entries[keyId])), SecurityAlgorithms.HmacSha256);
        }
    }
}
