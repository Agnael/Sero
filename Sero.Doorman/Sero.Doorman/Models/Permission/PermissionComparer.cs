
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sero.Doorman
{
    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals([AllowNull] Permission x, [AllowNull] Permission y)
        {
            return x.ResourceCode == y.ResourceCode
                && x.Level == y.Level;
        }

        public int GetHashCode([DisallowNull] Permission obj)
        {
            return obj.ResourceCode.GetHashCode()
                ^ obj.Level.GetHashCode();
        }
    }
}
