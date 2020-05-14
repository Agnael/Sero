using Sero.Sentinel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OkAuth.API
{
    public class CredentialVmBuilder
    {
        private CredentialVM _credential;

        public CredentialVmBuilder(string credentialId)
        {
            _credential = new CredentialVM();
            _credential.BirthDate = new DateTime(1990, 6, 1);
            _credential.CreationDate = DateTime.UtcNow;
            _credential.CredentialId = credentialId;
            _credential.DisplayName = credentialId;
            _credential.Email = credentialId + "@mail.com";
        }

        public CredentialVmBuilder WithBirthdate(int year, int month, int day)
        {
            _credential.BirthDate = new DateTime(year, month, day);
            return this;
        }

        public CredentialVmBuilder WithCreationDate(int year, int month, int day)
        {
            _credential.CreationDate = new DateTime(year, month, day);
            return this;
        }

        public CredentialVmBuilder WithDisplayName(string displayName)
        {
            _credential.DisplayName = displayName;
            return this;
        }

        public CredentialVmBuilder WithEmail(string email)
        {
            _credential.Email = email;
            return this;
        }

        public CredentialVmBuilder AddRole(CredentialRoleVm role)
        {
            _credential.Roles.Add(role);
            return this;
        }

        public CredentialVM Build()
        {
            return _credential;
        }
    }
}
