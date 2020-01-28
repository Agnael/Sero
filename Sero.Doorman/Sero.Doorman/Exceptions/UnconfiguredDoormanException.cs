using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class UnconfiguredDoormanException : Exception
    {
        public UnconfiguredDoormanException() 
            : base("Doorman error: No IOptionsMonitor<DoormanOptions> was injected by the DI container, please configure Doorman.'")
        {
        }
    }
}
