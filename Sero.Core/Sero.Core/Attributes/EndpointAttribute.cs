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
        public readonly EndpointRelation Relation;

        public EndpointAttribute(string resourceCode, EndpointScope actionScope, EndpointRelation relation)
        {
            this.ResourceCode = resourceCode;
            this.Scope = actionScope;
            this.Relation = relation;
        }
    }
}
