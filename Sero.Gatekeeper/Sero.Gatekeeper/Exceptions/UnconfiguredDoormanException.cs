using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class UnconfiguredGatekeeperException : Exception
    {
        public UnconfiguredGatekeeperException() 
            : base("Gatekeeper error: No IOptionsMonitor<GatekeeperOptions> was injected by the DI container, please configure Gatekeeper.'")
        {
        }
    }
}
