using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class ResourceComparer : IEqualityComparer<Resource>
    {
        public bool Equals([AllowNull] Resource x, [AllowNull] Resource y)
        {
            if (x == null && y == null)
                return true;

            return x.Category == y.Category
                && x.Code == y.Code
                && x.Description == y.Description;
        }

        public int GetHashCode([DisallowNull] Resource obj)
        {
            return obj.Category.GetHashCode()
                ^ obj.Code.GetHashCode()
                ^ obj.Description.GetHashCode();
        }
    }
}
