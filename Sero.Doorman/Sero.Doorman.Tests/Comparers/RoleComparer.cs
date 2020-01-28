
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Sero.Doorman.Tests.Comparers
{
    public class RoleComparer : IEqualityComparer<Role>
    {
        public readonly PermissionComparer PermissionComparer;

        public RoleComparer(PermissionComparer permissionComparer)
        {
            this.PermissionComparer = permissionComparer;
        }

        public bool Equals([AllowNull] Role x, [AllowNull] Role y)
        {
            return x.Code == y.Code
                && x.Description == y.Description
                && x.DisplayName == y.DisplayName
                && x.Permissions.SequenceEqual(y.Permissions, PermissionComparer);
        }

        public int GetHashCode([DisallowNull] Role obj)
        {
            return obj.Code.GetHashCode()
                ^ obj.Description.GetHashCode()
                ^ obj.DisplayName.GetHashCode()
                ^ obj.Permissions.GetHashCode();
        }
    }
}
