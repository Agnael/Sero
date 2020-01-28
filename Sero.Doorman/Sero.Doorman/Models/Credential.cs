using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class Credential
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }

        public List<Role> Roles { get; set; }

        public Credential()
        {
            Roles = new List<Role>();
        }
    }
}
