using Sero.Core;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel
{
    public class CredentialVM : IApiResource
    {
        public string OwnerId { get; set; }
        public string ApiResourceCode { get; set; }

        public string CredentialId { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public IList<CredentialRoleVm> Roles { get; set; }

        public CredentialVM()
        {

        }

        public CredentialVM(Credential credential)
        {
            this.OwnerId = credential.OwnerId;
            this.ApiResourceCode = credential.ApiResourceCode;
            this.CredentialId = credential.CredentialId;
            this.DisplayName = credential.DisplayName;
            this.CreationDate = credential.CreationDate;
            this.BirthDate = credential.BirthDate;
            this.Email = credential.Email;

            this.Roles = new List<CredentialRoleVm>();
            foreach(CredentialRole role in credential.Roles)
            {
                var newRoleVm = new CredentialRoleVm(role);
                this.Roles.Add(newRoleVm);
            }
        }
    }
}
