using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Sero.Sentinel
{
    public class CredentialRoleVmComparer : IEqualityComparer<CredentialRoleVm>
    {
        public CredentialRoleVmComparer()
        {

        }
        public bool Equals(CredentialRoleVm x, CredentialRoleVm y)
        {
            return
                x.Code.Equals(y.Code)
                && x.Description.Equals(y.Description);
        }

        public int GetHashCode(CredentialRoleVm obj)
        {
            return
                obj.Code.GetHashCode()
                ^ obj.Description.GetHashCode();
        }
    }
}
