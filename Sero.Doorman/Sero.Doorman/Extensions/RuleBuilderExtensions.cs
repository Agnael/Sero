using FluentValidation;
using Sero.Core;
using Sero.Core.Validation;
using Sero.Doorman.Controller;
using Sero.Doorman.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Doorman.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> IsCode<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(Constants.Validation.Code_Length_Min)
                .MaximumLength(Constants.Validation.Code_Length_Max)
                .Matches(string.Format("^[{0}]*$", Chars.Code));
        }

        public static IRuleBuilderOptions<T, string> IsDescription<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(Constants.Validation.Description_Length_Min)
                .MaximumLength(Constants.Validation.Description_Length_Max)
                .Matches(string.Format("^[{0}]*$", Chars.Description));
        }

        public static IRuleBuilderOptions<T, string> IsDisplayName<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(Constants.Validation.DisplayName_Length_Min)
                .MaximumLength(Constants.Validation.DisplayName_Length_Max)
                .Matches(string.Format("^[{0}]*$", Chars.DisplayName));
        }

        public static IRuleBuilderOptions<T, string> IsCSharpPropertyName<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .MaximumLength(255)
                .Matches(string.Format("^[{0}]+[{1}]*$", Chars.LETTERS, Chars.ALPHANUMERIC_UNDERSCORE));
        }

        public static IRuleBuilderOptions<T, string> IsOrderingDescriptor<T>(this IRuleBuilder<T, string> rule)
        {
            return rule.Must(BeIn(Order.ASC, Order.DESC, Order.Default));
        }

        public static IRuleBuilderOptions<T, string> IsPropertyNameOf<T>(this IRuleBuilder<T, string> rule, Type parentType)
        {
            return rule.Must(value => 
                {
                    bool isPropertyName = ReflectionUtils.HasProperty(parentType, value);
                    return isPropertyName;
                })  
                .WithMessage(string.Format("This value must be a property name of type '{0}'", parentType.FullName));
        }

        private static Func<string, bool> BeIn(params string[] whitelist)
        {
            return value => whitelist.Contains(value);
        }
    }
}
