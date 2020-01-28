using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Validation
{
    public class Chars
    {
        public const string LETTERS = 
            @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string INTEGERS = 
            @"0123456789";

        // El espacio está a propósito!!1
        public const string SPECIALS = 
            @".\-_ [\]/(),*<>@$&%#\\"; 

        public const string ALPHANUMERIC = 
            LETTERS + INTEGERS;

        public const string ALPHANUMERIC_UNDERSCORE = 
            ALPHANUMERIC+"_";

        public const string Code =
            ALPHANUMERIC_UNDERSCORE;

        public const string Description =
            LETTERS + INTEGERS + SPECIALS;

        // El espacio está a propósito!!1
        public const string DisplayName =
            ALPHANUMERIC_UNDERSCORE + " ()"; 
    }
}
