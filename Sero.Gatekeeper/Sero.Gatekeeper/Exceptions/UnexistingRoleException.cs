﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class UnexistingRoleException : Exception
    {
        public UnexistingRoleException() 
            : base("Gatekeeper error: No Role was found for a given role name.")
        {
        }
    }
}
