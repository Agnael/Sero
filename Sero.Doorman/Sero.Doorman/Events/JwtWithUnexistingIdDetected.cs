using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Sero.Doorman.Events
{
    public class JwtWithUnexistingIdDetected : Event
    {
        public JwtWithUnexistingIdDetected() 
            : base(LogLevel.Warning, 
                    Constants.LOXY_CATEGORY,
                    "JWT was correctly validated but its 'nameidentifier' claim had an unexisting IdUser value.")
        {
        }
    }
}
