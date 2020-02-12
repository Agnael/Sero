using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class DoormanActionAttribute : HateoasActionAttribute
    {
        public readonly PermissionLevel LevelRequired;

        public DoormanActionAttribute(string resourceCode, PermissionLevel levelRequired, ActionScope actionScope)
            : base(resourceCode, actionScope)
        {
            this.LevelRequired = levelRequired;
        }
    }
}
