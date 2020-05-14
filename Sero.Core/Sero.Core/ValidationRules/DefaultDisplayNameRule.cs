using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DefaultDisplayNameRule : IDisplayNameRule
    {
        public int LengthMin => CoreValidationConstants.DisplayName_Length_Min;
        public int LengthMax => CoreValidationConstants.DisplayName_Length_Max;

        private string _regexPattern;
        public string RegexPattern
        {
            get
            {
                if (string.IsNullOrEmpty(_regexPattern))
                    _regexPattern = string.Format("^[{0}]*$", Chars.DisplayName);

                return _regexPattern;
            }
        }

        public IRuleBuilder<T, string> ApplyRule<T>(IRuleBuilder<T, string> rule)
        {
            return rule
                .Length(LengthMin, LengthMax)
                .Matches(RegexPattern);
        }
    }
}
