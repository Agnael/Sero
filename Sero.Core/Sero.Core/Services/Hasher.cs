using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core.Services
{
    //public static class Hasher
    //{
    //    public static string GenerateSalt()
    //    {
    //        string allowedSaltChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //        int saltLength = 16;

    //        Random rand = new Random();
    //        char[] saltChars =
    //            Enumerable
    //            .Repeat(allowedSaltChars, saltLength)
    //            .Select(s =>
    //                s[rand.Next(s.Length)])
    //            .ToArray();

    //        return new string(saltChars);
    //    }

    //    public static string GenerateHash(string password, string passwordWideSalt, string appWideSalt)
    //    {
    //        if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
    //        if (string.IsNullOrEmpty(passwordWideSalt)) throw new ArgumentNullException(nameof(passwordWideSalt));
    //        if (string.IsNullOrEmpty(appWideSalt)) throw new ArgumentNullException(nameof(appWideSalt));

    //        string combinedPassword = password + passwordWideSalt + appWideSalt;

    //        var computedHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
    //        string hash = Convert.ToBase64String(computedHash);

    //        return hash;
    //    }
    //}
}
