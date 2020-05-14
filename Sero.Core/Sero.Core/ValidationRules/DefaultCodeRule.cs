using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DefaultCodeRule : ICodeRule
    {
        public IRuleBuilder<T, string> ApplyRule<T>(IRuleBuilder<T, string> rule)
        {
            return rule
                .MinimumLength(CoreValidationConstants.Code_Length_Min)
                .MaximumLength(CoreValidationConstants.Code_Length_Max)
                .Matches(string.Format("^[{0}]*$", Chars.Code));
        }
    }
}
