
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Sero.Doorman
{
    public class CredentialVmComparer : IEqualityComparer<CredentialVM>
    {
        public readonly RoleVmComparer RoleVmComparer;

        public CredentialVmComparer(RoleVmComparer roleVmComparer)
        {
            this.RoleVmComparer = roleVmComparer;
        }

        public bool Equals([AllowNull] CredentialVM x, [AllowNull] CredentialVM y)
        {
            return x.BirthDate.Equals(y.BirthDate)
                && x.CreationDate.Equals(y.CreationDate)
                && x.CredentialId == y.CredentialId
                && x.Email == y.Email
                && x.Roles.SequenceEqual(y.Roles, RoleVmComparer);
        }

        public int GetHashCode([DisallowNull] CredentialVM obj)
        {
            return obj.BirthDate.GetHashCode()
                ^ obj.CreationDate.GetHashCode()
                ^ obj.CredentialId.GetHashCode()
                ^ obj.Email.GetHashCode()
                ^ obj.Roles.GetHashCode();
        }
    }
}
