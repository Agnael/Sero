using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public static class DateTimeExtensions
    {
        public static string ToStandardDate(this DateTime dt)
        {
            string dateStr = dt.ToString("yyyy-MM-dd");
            return dateStr;
        }

        public static string ToStandardDate(this DateTime? dt)
        {
            if (!dt.HasValue)
                return null;

            string dateStr = dt.Value.ToStandardDate();
            return dateStr;
        }
    }
}
