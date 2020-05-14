using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sero.Core
{
    public static class ObjectExtensions
    {
        public static string ToUrlFriendlyValue(this Order order)
        {
            var str = order.ToString().ToAntiCapitalized();
            return str;
        }

        public static string ToUrlFriendlyValue(this int integer)
        {
            var str = integer.ToString();
            return str;
        }

        public static string ToUrlFriendlyValue<TEnum>(this TEnum enumValue)
            where TEnum: IComparable, IFormattable, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("en must be enum type");

            var str = enumValue.ToString().ToLower();
            return str;
        }

        public static string ToUrlFriendlyValue(this DateTime dt)
        {
            var str = dt.ToStandardDate();
            return str;
        }

        public static string ToUrlFriendlyValue(this DateTime? dt)
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

        public static string GetPropertyName<T, TProp>(this T target, Expression<Func<T, TProp>> propSelector)
        {
            var member = propSelector.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        public static TProp GetPropertyValue<TObj, TProp>(this TObj target, Expression<Func<TObj, TProp>> propSelector)
        {
            MemberExpression memberExpr = (MemberExpression)propSelector.Body;
            string memberName = memberExpr.Member.Name;
            Func<TObj, TProp> compiledDelegate = propSelector.Compile();
            TProp value = compiledDelegate(target);

            return value;
        }
    }
}
