using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class UnexistingResourceException : Exception
    {
        public UnexistingResourceException() 
            : base("Doorman error: No resource found for the name defined by the '[Sero.Doorman.Attributes.ResourceName(string name)] method attribute.'")
        {
        }
    }
}
