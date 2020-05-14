using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class NoJwtSent : Event
    {
        public NoJwtSent() 
            : base(LogLevel.Information, 
                  GtkConstants.LOXY_CATEGORY, 
                  "No JWT was written to the response.")
        {
        }
    }
}
