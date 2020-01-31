using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class UninitializedDoormanActionArgumentHolderService : Exception
    {
        public UninitializedDoormanActionArgumentHolderService() 
            : base("Doorman error: Please make sure you registered the DoormanActionFilter to the AspNetCore MVC pipeline.'")
        {
        }
    }
}
