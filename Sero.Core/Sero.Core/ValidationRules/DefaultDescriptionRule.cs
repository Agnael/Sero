using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DefaultDescriptionRule : IDescriptionRule
    {
        public IRuleBuilder<T, string> ApplyRule<T>(IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(CoreValidationConstants.Description_Length_Min)
                .MaximumLength(CoreValidationConstants.Description_Length_Max)
                .Matches(string.Format("^[{0}]*$", Chars.Description));
        }
    }
}
