using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class UserDetected : JsonStateEvent<ClaimsPrincipal>
    {
        public UserDetected(ClaimsPrincipal state) 
            : base(LogLevel.Information, 
                  GtkConstants.LOXY_CATEGORY, 
                  "The current request was authenticated succesfully.", 
                  state)
        {
        }
    }
}
