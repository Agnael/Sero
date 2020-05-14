using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class CredentialBuilder
    {
        private Credential _credential;

        public CredentialBuilder(string credentialId)
        {
            _credential = new Credential();
            _credential.BirthDate = new DateTime(1990, 6, 1);
            _credential.CreationDate = DateTime.UtcNow;
            _credential.CredentialId = credentialId;
            _credential.DisplayName = credentialId;
            _credential.Email = credentialId + "@mail.com";
            _credential.PasswordSalt = HashingUtil.GenerateSalt();
            _credential.PasswordHash = HashingUtil.GenerateHash("12345678", _credential.PasswordSalt);
        }

        public CredentialBuilder WithBirthdate(int year, int month, int day)
        {
            _credential.BirthDate = new DateTime(year, month, day);
            return this;
        }

        public CredentialBuilder WithCreationDate(int year, int month, int day)
        {
            _credential.CreationDate = new DateTime(year, month, day);
            return this;
        }

        public CredentialBuilder WithDisplayName(string displayName)
        {
            _credential.DisplayName = displayName;
            return this;
        }

        public CredentialBuilder WithEmail(string email)
        {
            _credential.Email = email;
            return this;
        }

        public CredentialBuilder WithPassword(string unhashedPassword)
        {
            _credential.PasswordSalt = HashingUtil.GenerateSalt();
            _credential.PasswordHash = HashingUtil.GenerateHash(unhashedPassword, _credential.PasswordSalt);
            return this;
        }

        //public CredentialBuilder AddRole(Role role)
        //{
        //    _credential.Roles.Add(role);
        //    return this;
        //}

        public Credential Build()
        {
            return _credential;
        }
    }
}
