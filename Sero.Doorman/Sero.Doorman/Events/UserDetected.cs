using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Doorman.Events
{
    public class UserDetected : JsonStateEvent<ClaimsPrincipal>
    {
        public UserDetected(ClaimsPrincipal state) 
            : base(LogLevel.Information, 
                  Constants.LOXY_CATEGORY, 
                  "The current request was authenticated succesfully.", 
                  state)
        {
        }
    }
}
