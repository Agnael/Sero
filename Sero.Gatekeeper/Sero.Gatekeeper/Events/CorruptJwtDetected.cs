using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class CorruptJwtDetected : Event
    {
        public CorruptJwtDetected(Exception ex) 
            : base(LogLevel.Error, GtkConstants.LOXY_CATEGORY, ex)
        {
        }
    }
}
