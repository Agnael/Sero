using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class UnexistingRoleException : Exception
    {
        public UnexistingRoleException() 
            : base("Doorman error: No Role was found for a given role name.")
        {
        }
    }
}
