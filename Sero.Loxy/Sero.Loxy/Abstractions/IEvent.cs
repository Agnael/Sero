using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IEvent
    {
        string Level { get; }
        string Category { get; }
        string Message { get; }
        string Type { get; }
        DateTime DateTime { get; }

        IEnumerable<string> Details { get; }
        IEnumerable<ExceptionInfo> Exception { get; }

        LogLevel GetLogLevel();

        // TODO: Buscar una forma de dejar de depender de este PREPARE de mierda
        void Prepare();
    }
}
