using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core.Utils
{
    public static class CasingUtil
    {
        public static string UpperCamelCaseToLowerUnderscore(string uperCamelCaseStr)
        {
            IEnumerable<string> parts = uperCamelCaseStr.Select(
                (x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()
            );

            string result = string.Concat(parts);
            return result.ToLower();
        }
    }
}
