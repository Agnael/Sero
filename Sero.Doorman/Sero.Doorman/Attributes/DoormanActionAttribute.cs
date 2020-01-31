using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class DoormanActionAttribute : Attribute
    {
        public readonly string ResourceCode;
        public readonly PermissionLevel LevelRequired;
        public readonly ActionScope ActionScope;

        public DoormanActionAttribute(string actionCode, string resourceCode, PermissionLevel levelRequired, ActionScope actionScope)
        {
            this.ResourceCode = resourceCode;
            this.LevelRequired = levelRequired;
            this.ActionScope = actionScope;
        }
    }
}
