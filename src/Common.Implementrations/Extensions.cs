using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Implementrations
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] bytes, bool lowercase = false)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            if (lowercase)
            {
                foreach (byte b in bytes)
                    hex.AppendFormat("{0:x2}", b);
            } 
            else
            {
                foreach (byte b in bytes)
                    hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        public static string NextString(this Random random, int length, string allowedCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        {
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(allowedCharacters[random.Next(allowedCharacters.Length)]);
            }
            return result.ToString();
        }

    }
}
