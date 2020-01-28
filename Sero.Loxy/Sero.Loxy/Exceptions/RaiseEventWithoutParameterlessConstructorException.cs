using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class RaiseEventWithoutParameterlessConstructorException : Exception
    {
        public RaiseEventWithoutParameterlessConstructorException() 
            : base("Can't implicitly activate this IEvent type without a parameterless constructor.")
        {
        }
    }
}
