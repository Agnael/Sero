using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class UnsecuredActionException : Exception
    {
        public UnsecuredActionException() 
            : base("Doorman error: Even if the resource is meant to be public, it must have a name determined by the '[Sero.Doorman.Attributes.ResourceName(string name)] method attribute.'")
        {
        }
    }
}
