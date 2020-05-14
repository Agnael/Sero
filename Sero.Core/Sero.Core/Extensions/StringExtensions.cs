using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sero.Core
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Returns the string with the first character as lower-case.
        /// </summary>
        /// <returns></returns>
        public static string ToAntiCapitalized(this string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
        }
        
        public static bool IsEmail(this string value)
        {
            var regex = new Regex(CoreValidationConstants.Email_RegexPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }
    }
}
