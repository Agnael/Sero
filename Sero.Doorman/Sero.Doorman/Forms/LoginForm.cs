using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class LoginForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public LoginForm()
        {

        }
    }
}
