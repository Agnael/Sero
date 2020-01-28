using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Proxies
{
    class ProxyStateFormatterFactory : IStateFormatterFactory
    {
        IStateFormatter<TState> IStateFormatterFactory.Create<TState>(EventId loggedEventId, Func<TState, Exception, string> defaultStateFormatter)
        {
            var stateFormatter = new ProxyStateFormatter<TState>(loggedEventId, defaultStateFormatter);
            return stateFormatter;
        }
    }
}
