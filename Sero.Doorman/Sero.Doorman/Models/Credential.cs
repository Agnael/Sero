using Sero.Core;
using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class Credential : Element
    {
        public Guid CredentialId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }

        public List<Role> Roles { get; set; }

        public Credential() 
            : base(Constants.ResourceCodes.Credentials)
        {
            Roles = new List<Role>();
        }
    }
}
