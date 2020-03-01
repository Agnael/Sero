using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.ViewModels
{
    public class CredentialViewModel
    {
        public Guid CredentialId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public IList<RoleViewModel> Roles { get; set; }

        public CredentialViewModel(Credential credential)
        {
            this.CredentialId = credential.CredentialId;
            this.CreationDate = credential.CreationDate;
            this.BirthDate = credential.BirthDate;
            this.Email = credential.Email;

            this.Roles = new List<RoleViewModel>();
            foreach(Role role in credential.Roles)
            {
                var newRoleVm = new RoleViewModel(role);
                this.Roles.Add(newRoleVm);
            }
        }
    }
}
