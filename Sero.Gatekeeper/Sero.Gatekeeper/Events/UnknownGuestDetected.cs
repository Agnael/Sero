using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class UnknownGuestDetected : Event
    {
        public UnknownGuestDetected() 
            : base(LogLevel.Information, 
                  GtkConstants.LOXY_CATEGORY, 
                  "The current request could not be authenticated, the requester will be treated as an unknown guest.")
        {
        }
    }
}
