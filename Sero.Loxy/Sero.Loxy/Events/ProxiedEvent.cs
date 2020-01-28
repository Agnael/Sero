using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Events
{
    public class ProxiedEvent<TState> : AbstractEvent<TState>
    {
        IStateFormatter<TState> _stateFormatter;
        
        public ProxiedEvent(LogLevel level, 
                            string category, 
                            string message, 
                            TState state, 
                            IStateFormatter<TState> customStateFormatter)
            : base(level, category, message, state)
        {
            if (customStateFormatter == null) throw new ArgumentNullException("customStateFormatter");
            _stateFormatter = customStateFormatter;
        }

        protected override IEnumerable<string> FormatState(TState state)
        {
            IEnumerable<string> formatted = _stateFormatter.Format(state);
            return formatted;
        }
    }
}
