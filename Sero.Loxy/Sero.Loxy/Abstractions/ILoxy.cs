using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy.Abstractions
{
    public interface ILoxy
    {
        void Raise(IEvent evt);
        void Raise<T>() where T : IEvent;
        Task PersistAsync();

        ILoggerFactory AsLoggerFactory();
        ILoggerFactory AsLoggerFactory(LoggerProxyOptions options);
        ILoggerFactory AsLoggerFactory(Action<LoggerProxyOptions> config);

        ILoggerProvider AsLoggerProvider();
        ILoggerProvider AsLoggerProvider(LoggerProxyOptions options);
        ILoggerProvider AsLoggerProvider(Action<LoggerProxyOptions> config);

        ILogger AsLogger();
        ILogger AsLogger(LoggerProxyOptions options);
        ILogger AsLogger(Action<LoggerProxyOptions> config);
        
        IList<ISink> Sinks { get; }
    }
}
