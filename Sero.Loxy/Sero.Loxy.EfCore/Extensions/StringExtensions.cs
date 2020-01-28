using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore.Extensions
{
    internal static class StringExtensions
    {
        /// <summary>
        ///     If there is more than  consecutive whitespace, they are replaced with a single one.
        /// </summary>
        public static string RemoveExtraWhitespaces(this string value)
        {
            StringBuilder tmpbuilder = new StringBuilder(value.Length);

            bool inspaces = false;
            tmpbuilder.Length = 0;

            for (int k = 0; k < value.Length; ++k)
            {
                char c = value[k];

                if (inspaces)
                {
                    if (c != ' ')
                    {
                        inspaces = false;
                        tmpbuilder.Append(c);
                    }
                }
                else if (c == ' ')
                {
                    inspaces = true;
                    tmpbuilder.Append(' ');
                }
                else
                    tmpbuilder.Append(c);
            }

            string result = tmpbuilder.ToString();
            return result;
        }

        public static string UnescapeDoubleQuotes(this string value)
        {
            return value.Replace("\"", "'");
        }

        public static string ReplaceParam(this string tmpl, string paramName, string value)
        {
            if (paramName == null) throw new ArgumentNullException(nameof(paramName));
            if (value == null) throw new ArgumentNullException(nameof(value));

            return tmpl.Replace("{" + paramName + "}", value);
        }

        public static string ReplaceParam(this string tmpl, KeyValuePair<string, object> paramKvp)
        {
            if (FormattingUtils.IsNullKvp(paramKvp)) throw new ArgumentNullException(nameof(paramKvp));
            
            return tmpl.Replace("{" + paramKvp.Key + "}", paramKvp.Value.ToString());
        }

        public static IEnumerable<string> SplitByNewLine(this string value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return value.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
