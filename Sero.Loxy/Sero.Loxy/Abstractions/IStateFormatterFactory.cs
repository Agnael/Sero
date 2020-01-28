using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IStateFormatterFactory
    {
        IStateFormatter<TState> Create<TState>(EventId loggedEventId, Func<TState, Exception, string> defaultStateFormatter);
    }
}
