using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Doorman.Events
{
    public class CorruptJwtDetected : Event
    {
        public CorruptJwtDetected(Exception ex) 
            : base(LogLevel.Error, Constants.LOXY_CATEGORY, ex)
        {
        }
    }
}
