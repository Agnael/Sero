using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class CredentialVM
    {
        public string CredentialId { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public IList<RoleVM> Roles { get; set; }

        public CredentialVM()
        {

        }

        public CredentialVM(Credential credential)
        {
            this.CredentialId = credential.CredentialId;
            this.DisplayName = credential.DisplayName;
            this.CreationDate = credential.CreationDate;
            this.BirthDate = credential.BirthDate;
            this.Email = credential.Email;

            this.Roles = new List<RoleVM>();
            foreach(Role role in credential.Roles)
            {
                var newRoleVm = new RoleVM(role);
                this.Roles.Add(newRoleVm);
            }
        }
    }
}
