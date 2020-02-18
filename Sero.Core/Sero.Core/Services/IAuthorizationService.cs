using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface ISecurityService
    {
        bool IsAuthorized(string resourceCode, PermissionLevel requiredLevel);
    }
}
