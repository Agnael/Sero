using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Doorman.Events
{
    public class NoJwtSent : Event
    {
        public NoJwtSent() 
            : base(LogLevel.Information, 
                  Constants.LOXY_CATEGORY, 
                  "No JWT was written to the response.")
        {
        }
    }
}
