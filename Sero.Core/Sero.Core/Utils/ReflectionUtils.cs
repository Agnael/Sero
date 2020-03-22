using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        ///     Dado un string que tenga placeholders especificados con el formato "{property}", reemplaza dichos placeholders
        ///     con los VALORES de las properties del parámetro valuesSource que tengan el mismo nombre que un placeholder existente
        ///     en el string.
        /// </summary>
        public static string ReplaceUrlTemplate(string urlTemplate, object valuesSource)
        {
            string result = urlTemplate;

            if (valuesSource != null)
            {
                // Este regex trae el texto ENTRE llaves {} pero sin las llaves
                Regex regex = new Regex(@"(?<={)(.*?)(?=})");
                var match = regex.Match(result);

                if (match.Success)
                {
                    var elementPropList = valuesSource.GetType().GetProperties();

                    foreach (Group matchedGroup in match.Groups)
                    {
                        string urlParam = matchedGroup.Value?.ToLower();
                        PropertyInfo propInfo = elementPropList.FirstOrDefault(x => x.Name.ToLower() == urlParam);
                        object foundValue = propInfo.GetValue(valuesSource, null);

                        if (propInfo != null)
                            result = result.Replace(string.Format("{{{0}}}", matchedGroup.Value), foundValue?.ToString());
                    }
                }
            }

            return result;
        }

        public static string ReplaceUrlTemplate(string urlTemplate, string replacementKey, object replacementValue)
        {
            if (string.IsNullOrEmpty(urlTemplate)) throw new ArgumentNullException(nameof(urlTemplate));
            if (string.IsNullOrEmpty(replacementKey)) throw new ArgumentNullException(nameof(replacementKey));
            if (replacementValue == null) throw new ArgumentNullException(nameof(replacementValue));

            string result = urlTemplate;

            // Este regex trae el texto ENTRE llaves {} pero sin las llaves
            Regex regex = new Regex("{" + replacementKey + "}");
            var match = regex.Match(result);

            if (match.Success)
            {
                foreach (Group matchedGroup in match.Groups)
                    result = result.Replace(matchedGroup.Value, replacementValue.ToString());
            }
            else
            {
                throw new Exception("El template de URL que se intenta usar no contiene un placeholder para la key que se está buscando reemplazar.");
            }

            return result;
        }
    }
}
