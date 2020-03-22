using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class GetterAttribute : Attribute
    {
        public readonly string DisplayNameWhenLinked;

        public GetterAttribute(string displaynameWhenParent)
        {
            this.DisplayNameWhenLinked = CasingUtil.UpperCamelCaseToLowerUnderscore(displaynameWhenParent);
        }
    }
}
