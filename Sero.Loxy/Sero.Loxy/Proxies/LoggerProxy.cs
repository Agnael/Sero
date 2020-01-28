using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoggerProxy : ILogger
    {
        private readonly ILoxy _loxy;
        public readonly LoggerProxyOptions Options;

        public LoggerProxy(ILoxy loxy)
        {
            _loxy = loxy;
            Options = new LoggerProxyOptions();
        }

        public LoggerProxy(ILoxy loxy, Action<LoggerProxyOptions> config)
        {
            LoggerProxyOptions defaultOptions = new LoggerProxyOptions();
            config(defaultOptions);

            _loxy = loxy;
            Options = defaultOptions;
        }

        public LoggerProxy(ILoxy loxy, LoggerProxyOptions options)
        {
            _loxy = loxy;
            Options = options;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            var dummy = state as IDisposable;
            return dummy;
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            // Siempre permite TODOS los eventos. Este ILogger solo es un facade porque EFCore fuerza usar esto 
            // pero el que decide realmente qué se escribe y qué no es el ILoxy.
            return true;
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            IStateFormatter<TState> stateFormatter = Options.StateFormatterFactory.Create<TState>(eventId, formatter);
            ProxiedEvent<TState> evt = new ProxiedEvent<TState>(logLevel, Options.Category, eventId.Name, state, stateFormatter);

            _loxy.Raise(evt);
        }
    }
}
