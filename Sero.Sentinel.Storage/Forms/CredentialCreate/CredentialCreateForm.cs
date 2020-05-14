using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class CredentialCreateForm
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public DateTime Birthdate { get; set; }

        public CredentialCreateForm()
        {

        }
    }
}
