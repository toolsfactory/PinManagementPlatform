using System;
using System.Security.Cryptography;
using System.Text;
using Tiveria.Common.Extensions;

namespace PinPlatform.Common.Implementrations
{
    public class PinHashGenerator : IPinHashGenerator
    {
        private readonly Random _rand;
        private readonly SHA256 _sha;

        public PinHashGenerator()
        {
            _rand = new Random();
            _sha = SHA256.Create();
        }
        public string GeneratePinHash(string pin, string salt)
        {
            return _sha.ComputeHash(Encoding.ASCII.GetBytes(pin + salt)).ToHexString();
        }

        public (string Hash, string Salt) GeneratePinHashWithNewSalt(string pin)
        {
            var salt = _rand.NextString(32);
            var hash = GeneratePinHash(pin, salt);
            return (hash, salt);
        }
    }
}
