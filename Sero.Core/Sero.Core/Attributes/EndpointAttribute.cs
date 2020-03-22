using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class EndpointAttribute : Attribute
    {
        public readonly string ResourceCode;
        public readonly EndpointScope Scope;

        public EndpointAttribute(string resourceCode, EndpointScope actionScope)
        {
            this.ResourceCode = resourceCode;
            this.Scope = actionScope;
        }
    }
}
