using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class UnhandledEfEventException : Exception
    {
        public UnhandledEfEventException() 
            : base("It was not possible to get enough descriptive information from this SqlCommand.")
        {
        }
    }
}
