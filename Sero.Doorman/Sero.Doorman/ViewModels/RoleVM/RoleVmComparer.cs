using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sero.Doorman
{
    public class RoleVmComparer : IEqualityComparer<RoleVM>
    {
        public RoleVmComparer()
        {

        }

        public bool Equals([AllowNull] RoleVM x, [AllowNull] RoleVM y)
        {
            return x.Code == y.Code
                && x.DisplayName == y.DisplayName;
        }

        public int GetHashCode([DisallowNull] RoleVM obj)
        {
            return obj.Code.GetHashCode()
                ^ obj.DisplayName.GetHashCode();
        }
    }
}
