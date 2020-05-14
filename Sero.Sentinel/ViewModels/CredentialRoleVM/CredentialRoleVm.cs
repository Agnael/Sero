using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel
{
    public class CredentialRoleVm
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public CredentialRoleVm()
        {

        }

        public CredentialRoleVm(CredentialRole credentialRole)
        {
            this.Code = credentialRole.Code;
            this.Description = credentialRole.Description;
        }
    }
}
