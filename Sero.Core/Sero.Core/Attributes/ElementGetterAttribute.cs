using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ElementGetterAttribute : Attribute
    {
        public readonly string DisplayNameWhenParent;

        public ElementGetterAttribute(string displaynameWhenParent)
        {
            this.DisplayNameWhenParent = CasingUtil.UpperCamelCaseToLowerUnderscore(displaynameWhenParent);
        }
    }
}
