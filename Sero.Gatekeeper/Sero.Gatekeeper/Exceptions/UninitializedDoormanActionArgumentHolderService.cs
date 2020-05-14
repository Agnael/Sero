using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class UninitializedGatekeeperActionArgumentHolderService : Exception
    {
        public UninitializedGatekeeperActionArgumentHolderService() 
            : base("Gatekeeper error: Please make sure you registered the GatekeeperActionFilter to the AspNetCore MVC pipeline.'")
        {
        }
    }
}
