﻿using Microsoft.Extensions.Logging;
using Sero.Gatekeeper.Storage;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Gatekeeper.Events
{
    public class JwtSent : JsonStateEvent<string>
    {
        public JwtSent(string token) 
            : base(LogLevel.Information, 
                  GtkConstants.LOXY_CATEGORY, 
                  "A JWT was generated using the current Identity.",
                  token)
        {
        }
    }
}
