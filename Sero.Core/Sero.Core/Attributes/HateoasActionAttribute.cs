using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class HateoasActionAttribute : Attribute
    {
        public readonly string ResourceCode;
        public readonly ActionScope ActionScope;

        public HateoasActionAttribute(string resourceCode, ActionScope actionScope)
        {
            this.ResourceCode = resourceCode;
            this.ActionScope = actionScope;
        }
    }
}
