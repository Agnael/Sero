using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class SafeEndpointAttribute : EndpointAttribute
    {
        public readonly PermissionLevel LevelRequired;

        public SafeEndpointAttribute(string resourceCode, PermissionLevel levelRequired, EndpointScope actionScope)
            : base(resourceCode, actionScope)
        {
            this.LevelRequired = levelRequired;
        }
    }
}
