using Fare;
using Sero.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public static class ValUtil
    {
        private static Random Random = new Random();
        
        public static string GenerateString(string allowedCharacters, uint length, string forcedChars = null)
        {
            StringBuilder resultStringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
                resultStringBuilder.Append(allowedCharacters[Random.Next(allowedCharacters.Length)]);

            string result = resultStringBuilder.ToString();

            if(forcedChars != null)
            {
                result = result.Substring(0, result.Length - forcedChars.Length);
                result = forcedChars + result;
            }

            return result;
        }

        public static string GetCode()
        {
            return GetCode(Constants.Validation.Code_Length_Max);
        }

        public static string GetCode(string forcedChars)
        {
            return GetCode(Constants.Validation.Code_Length_Max, forcedChars);
        }

        public static string GetCode(uint length, string forcedChars = null)
        {
            return GenerateString(Chars.Code, length, forcedChars);
        }

        public static string GetDisplayName()
        {
            return GetDisplayName(Constants.Validation.DisplayName_Length_Max);
        }

        public static string GetDisplayName(string forcedChars)
        {
            return GetDisplayName(Constants.Validation.DisplayName_Length_Max, forcedChars);
        }

        public static string GetDisplayName(uint length, string forcedChars = null)
        {
            return GenerateString(Chars.DisplayName, length, forcedChars);
        }

        public static string GetDescription()
        {
            return GetDescription(Constants.Validation.Description_Length_Max);
        }

        public static string GetDescription(string forcedChars)
        {
            return GetDescription(Constants.Validation.Description_Length_Max, forcedChars);
        }

        public static string GetDescription(uint length, string forcedChars = null)
        {
            return GenerateString(Chars.Description, length, forcedChars);
        }
    }
}
