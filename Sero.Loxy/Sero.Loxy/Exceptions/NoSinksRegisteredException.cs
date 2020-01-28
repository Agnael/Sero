using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class NoSinksRegisteredException : Exception
    {
        public NoSinksRegisteredException() 
            : base("To use Loxy, you must register at least one Sink.")
        {
        }
    }
}
