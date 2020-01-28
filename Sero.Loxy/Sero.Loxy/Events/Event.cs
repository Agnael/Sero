using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Events
{
    public class Event : AbstractEvent
    {
        public Event(LogLevel level, string category, string message) : base(level, category, message)
        {
        }

        public Event(LogLevel level, string category, Exception ex) : base(level, category, ex)
        {
        }

        public Event(LogLevel level, string category, string message, Exception ex) : base(level, category, message, ex)
        {
        }
    }
}
