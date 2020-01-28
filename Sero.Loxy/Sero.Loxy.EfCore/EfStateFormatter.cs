using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using Sero.Loxy.EfCore.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class EfStateFormatter<TState> : IStateFormatter<TState>
    {
        private EventId _eventId;
        private Func<TState, Exception, string> _defaultFormatter;

        public EfStateFormatter(EventId loggedEventId, Func<TState, Exception, string> defaultFormatter)
        {
            _eventId = loggedEventId;
            _defaultFormatter = defaultFormatter;
        }

        public IEnumerable<string> Format(TState state)
        {
            IEnumerable<string> details = null;

            // EF log customizations
            if (_eventId == RelationalEventId.CommandExecuted)
            {
                details = FormattingUtils.FormatCommandExecuted<TState>(state, _defaultFormatter);
            }
            else if(_eventId == RelationalEventId.CommandExecuting)
            {
                details = FormattingUtils.FormatCommandExecuting<TState>(state, _defaultFormatter);
            }
            else // It's an EF log we don't want to customize
            {
                string formatted = _defaultFormatter(state, null);

                if (!string.IsNullOrEmpty(formatted))
                {
                    details = formatted.RemoveExtraWhitespaces()
                                        .SplitByNewLine()
                                        .Select(x => x.Trim());
                }
            }

            if (details == null)
                throw new UnhandledEfEventException();

            return details;
        }
    }
}
