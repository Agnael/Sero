using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sero.Doorman
{
    internal static class HashingUtil
    {
        public static string GenerateSalt()
        {
            Random rand = new Random();
            char[] salt = Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 16)
                                    .Select(s => s[rand.Next(s.Length)])
                                    .ToArray();

            return new string(salt);
        }

        public static string GenerateHash(string password, string salt)
        {
            var computedHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(salt + password));
            return Convert.ToBase64String(computedHash);
        }
    }
}
