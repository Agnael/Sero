using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Proxies;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoggerProxyOptions
    {
        public string Category { get; private set; }

        /// <summary>
        /// Useful to track the relation between logs when there are multiple similar clients performing similar tasks (i.e.: Multiple DbContexts performing queries).
        /// </summary>
        public Guid? LoggerProviderId { get; private set; }

        public IExceptionFormatter ExceptionFormatter { get; private set; }

        //public IStateFormatter StateFormatterOverride { get; private set; }
        public IStateFormatterFactory StateFormatterFactory { get; private set; }

        public LoggerProxyOptions()
        {
            Category = "UNCATEGORIZED";
            StateFormatterFactory = new ProxyStateFormatterFactory();
        }

        public LoggerProxyOptions(Guid loggerProviderId)
        {
            Category = "UNCATEGORIZED";
            LoggerProviderId = LoggerProviderId;
            StateFormatterFactory = new ProxyStateFormatterFactory();
        }

        public LoggerProxyOptions WithLoggerProviderId(Guid loggerProviderId)
        {
            if(LoggerProviderId.HasValue)
                return this;

            LoggerProviderId = loggerProviderId;
            return this;
        }

        public LoggerProxyOptions WithCategory(string category)
        {
            Category = category;
            return this;
        }

        public LoggerProxyOptions WithExceptionFormatter(IExceptionFormatter customExceptionFormatter)
        {
            ExceptionFormatter = customExceptionFormatter;
            return this;
        }

        public LoggerProxyOptions WithStateFormatterFactory(IStateFormatterFactory stateFormatterFactory)
        {
            StateFormatterFactory = stateFormatterFactory;
            return this;
        }
    }
}
