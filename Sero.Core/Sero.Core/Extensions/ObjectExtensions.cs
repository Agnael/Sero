using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sero.Core
{
    public static class ObjectExtensions
    {
        public static string ToFilterString(this Order order)
        {
            var str = order.ToString().ToAntiCapitalized();
            return str;
        }

        public static string ToFilterString(this int integer)
        {
            var str = integer.ToString();
            return str;
        }

        public static string ToFilterString<TEnum>(this TEnum enumValue)
            where TEnum: IComparable, IFormattable, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("en must be enum type");

            var str = enumValue.ToString().ToLower();
            return str;
        }

        public static string ToFilterString(this DateTime dt)
        {
            var str = dt.ToStandardDate();
            return str;
        }

        public static string ToFilterString(this DateTime? dt)
        {
            var str = dt.ToStandardDate();
            return str;
        }

        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}
