using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Sero.Core
{
    public static class ReflectionUtils
    {
        public static Func<TIn, string> GeneratePropertySelector<TIn>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TIn));
            var body = Expression.PropertyOrField(param, propertyName);
            return Expression.Lambda<Func<TIn, string>>(body, param).Compile();
        }

        public static string GetPropertyName<T, U>(Expression<Func<T, U>> propertyLambda)
        {
            var member = propertyLambda.Body as MemberExpression;
            if (member != null)
                return member.Member.Name;

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        public static bool HasProperty(Type objType, String name)
        {
            var parts = name.Split('.').ToList();
            var currentPart = parts[0];

            PropertyInfo info = objType.GetProperty(currentPart);

            if (info == null)
                return false;

            if (name.IndexOf(".") > -1)
            {
                parts.Remove(currentPart);
                return HasProperty(info.PropertyType, String.Join(".", parts));
            }
            else
            {
                return true;
            }
        }
    }
}
