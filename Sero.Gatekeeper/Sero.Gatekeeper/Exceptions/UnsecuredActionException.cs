using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class UnsecuredActionException : Exception
    {
        public UnsecuredActionException() 
            : base("Gatekeeper error: Even if the resource is meant to be public, it must have a name determined by the '[Sero.Gatekeeper.Attributes.ResourceName(string name)] method attribute.'")
        {
        }
    }
}
