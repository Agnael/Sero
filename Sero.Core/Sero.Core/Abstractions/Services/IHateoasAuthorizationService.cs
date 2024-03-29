﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Abstractions
{
    public interface IHateoasAuthorizationService
    {
        bool IsAuthorized(string requiredResourceCode, PermissionLevel requiredLevel);
    }
}
