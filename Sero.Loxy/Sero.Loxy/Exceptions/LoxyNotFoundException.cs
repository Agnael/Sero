using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoxyNotFoundException : Exception
    {
        public LoxyNotFoundException() 
            : base("To use Loxy's middleware, you must register Loxy as a service in the DI container.")
        {
        }
    }
}
