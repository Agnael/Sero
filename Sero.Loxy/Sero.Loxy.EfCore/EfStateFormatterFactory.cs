using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class EfStateFormatterFactory : IStateFormatterFactory
    {
        public IStateFormatter<TState> Create<TState>(EventId loggedEventId, Func<TState, Exception, string> defaultStateFormatter)
        {
            var formatter = new EfStateFormatter<TState>(loggedEventId, defaultStateFormatter);
            return formatter;
        }
    }
}
