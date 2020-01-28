using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Events
{
    public class UnknownGuestDetected : Event
    {
        public UnknownGuestDetected() 
            : base(LogLevel.Information, 
                  Constants.LOXY_CATEGORY, 
                  "The current request could not be authenticated, the requester will be treated as an unknown guest.")
        {
        }
    }
}
