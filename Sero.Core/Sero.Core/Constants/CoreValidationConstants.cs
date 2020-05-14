using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class CoreValidationConstants
    {
        public const int Code_Length_Min = 1;
        public const int Code_Length_Max = 25;

        public const int Description_Length_Min = 1;
        public const int Description_Length_Max = 255;

        public const int DisplayName_Length_Min = 3;
        public const int DisplayName_Length_Max = 30;

        public const string Email_RegexPattern = 
            @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
    }
}
