using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class UnexistingResourceException : Exception
    {
        public UnexistingResourceException() 
            : base("Gatekeeper error: No resource found for the name defined by the '[Sero.Gatekeeper.Attributes.ResourceName(string name)] method attribute.'")
        {
        }
    }
}
