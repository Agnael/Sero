using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class GatekeeperError : Event
    {
        public GatekeeperError(Exception ex) 
            : base(LogLevel.Critical, GtkConstants.LOXY_CATEGORY, ex)
        {
        }
    }
}
