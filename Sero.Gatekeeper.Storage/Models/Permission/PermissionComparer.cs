
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class PermissionComparer : IEqualityComparer<Permission>
    {
        public bool Equals([AllowNull] Permission x, [AllowNull] Permission y)
        {
            return x.ResourceCode == y.ResourceCode
                && x.LevelOnOwned == y.LevelOnOwned
                && x.LevelOnAny == y.LevelOnAny;
        }

        public int GetHashCode([DisallowNull] Permission obj)
        {
            return obj.ResourceCode.GetHashCode()
                ^ obj.LevelOnOwned.GetHashCode()
                ^ obj.LevelOnAny.GetHashCode();
        }
    }
}
