using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class RequirePermissionAttribute : Attribute
    {
        public readonly string ResourceCode;
        public readonly PermissionLevel LevelRequired;

        public RequirePermissionAttribute(string resourceCode, PermissionLevel levelRequired)
        {
            this.ResourceCode = resourceCode;
            this.LevelRequired = levelRequired;
        }
    }
}
