using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoggerProviderProxy : ILoggerProvider
    {
        public readonly Guid Id;

        protected LoggerProxy _dbLogger;
        protected readonly ILoxy _events;


        public readonly LoggerProxyOptions Options;

        private static object _lock = new object();

        public LoggerProviderProxy(ILoxy eventLogger, LoggerProxyOptions options)
        {
            Id = new Guid();
            _events = eventLogger;
            Options = options;
        }

        public LoggerProviderProxy(ILoxy eventLogger, Action<LoggerProxyOptions> config)
        {
            Id = new Guid();
            _events = eventLogger;
            
            LoggerProxyOptions defaultOptions = new LoggerProxyOptions();
            config(defaultOptions);
            Options = defaultOptions;
        }

        public LoggerProviderProxy(ILoxy eventLogger)
        {
            Id = new Guid();
            _events = eventLogger;
        }

        public void AddProvider(ILoggerProvider provider)
        {
            // Don't need to implement this, the facade relies on EventLogger only
        }

        public ILogger CreateLogger(string categoryName)
        {
            return GetLogger();
        }

        public void Dispose()
        {

        }

        private LoggerProxy GetLogger()
        {
            lock (_lock)
            {
                if (_dbLogger == null)
                    _dbLogger = new LoggerProxy(_events, Options);
            }

            return _dbLogger;
        }
    }
}
