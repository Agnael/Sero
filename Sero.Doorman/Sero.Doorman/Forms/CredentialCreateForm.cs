using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class CredentialCreateForm
    {
        public string CredentialId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public DateTime Birthdate { get; set; }

        public CredentialCreateForm()
        {

        }
    }
}
