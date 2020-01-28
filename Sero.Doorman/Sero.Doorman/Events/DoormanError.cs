using Microsoft.Extensions.Logging;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Events
{
    public class DoormanError : Event
    {
        public DoormanError(Exception ex) 
            : base(LogLevel.Critical, Constants.LOXY_CATEGORY, ex)
        {
        }
    }
}
