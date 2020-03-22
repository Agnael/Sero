﻿using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class DoormanEndpointAttribute : EndpointAttribute
    {
        public readonly PermissionLevel LevelRequired;

        public DoormanEndpointAttribute(string resourceCode, PermissionLevel levelRequired, EndpointScope actionScope)
            : base(resourceCode, actionScope)
        {
            this.LevelRequired = levelRequired;
        }
    }
}