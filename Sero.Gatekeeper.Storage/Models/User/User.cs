using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class User
    {
        public string OkAuthCredentialId { get; set; }

        public IList<Role> Roles { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
        }
    }
}
