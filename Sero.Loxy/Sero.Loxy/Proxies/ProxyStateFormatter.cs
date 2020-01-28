using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Proxies
{
    public class ProxyStateFormatter<TState> : IStateFormatter<TState>
    {
        private readonly EventId _eventId;
        private readonly Func<TState, Exception, string> _defaultFormatter;

        public ProxyStateFormatter(EventId loggedEventId, Func<TState, Exception, string> defaultStateFormatter)
        {
            _eventId = loggedEventId;
            _defaultFormatter = defaultStateFormatter;
        }

        IEnumerable<string> IStateFormatter<TState>.Format(TState state)
        {
            string formatted = _defaultFormatter(state, null);
            return new string[] { formatted };
        }
    }
}
