using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sero.Core;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy
{
    public class Loxy : ILoxy
    {
        public readonly List<IEvent> EventHistory;
        public readonly HttpContext HttpContext;
        public readonly IAppInfoService ApplicationInfoService;
        public readonly IRequestInfoService RequestInfoService;

        public IList<ISink> Sinks { get; private set; }
        
        public Loxy(IHttpContextAccessor httpContext, 
                            IAppInfoService appInfoService,
                            IRequestInfoService reqInfoService,
                            LoxyBuilder loxyBuilder)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            if (appInfoService == null) throw new ArgumentNullException(nameof(appInfoService));
            if (reqInfoService == null) throw new ArgumentNullException(nameof(reqInfoService));
            if (loxyBuilder == null) throw new ArgumentNullException(nameof(loxyBuilder));

            EventHistory = new List<IEvent>();
            HttpContext = httpContext.HttpContext;
            ApplicationInfoService = appInfoService;
            RequestInfoService = reqInfoService;
            Sinks = loxyBuilder.Sinks.Sinks;
        }

        private LoggerProxyOptions GenerateLoggerOptions()
        {
            LoggerProxyOptions defaultOptions = new LoggerProxyOptions();
            return defaultOptions;
        }

        private LoggerProxyOptions GenerateLoggerOptions(Action<LoggerProxyOptions> config)
        {
            LoggerProxyOptions defaultOptions = new LoggerProxyOptions();
            config(defaultOptions);

            return defaultOptions;
        }

        public ILogger AsLogger()
        {
            LoggerProxy logger = new LoggerProxy(this);
            return logger;
        }

        public ILogger AsLogger(LoggerProxyOptions options)
        {
            LoggerProxy logger = new LoggerProxy(this, options);
            return logger;
        }

        public ILogger AsLogger(Action<LoggerProxyOptions> config)
        {
            var options = GenerateLoggerOptions(config);

            LoggerProxy logger = new LoggerProxy(this, options);
            return logger;
        }

        public ILoggerFactory AsLoggerFactory()
        {
            LoggerFactoryProxy loggerFactoryFacade = new LoggerFactoryProxy(this);
            return loggerFactoryFacade;
        }

        public ILoggerFactory AsLoggerFactory(LoggerProxyOptions options)
        {
            LoggerFactoryProxy loggerFactoryFacade = new LoggerFactoryProxy(this, options);
            return loggerFactoryFacade;
        }

        public ILoggerFactory AsLoggerFactory(Action<LoggerProxyOptions> config)
        {
            var options = GenerateLoggerOptions(config);

            LoggerFactoryProxy logger = new LoggerFactoryProxy(this, options);
            return logger;
        }

        public ILoggerProvider AsLoggerProvider()
        {
            LoggerProviderProxy providerFacade = new LoggerProviderProxy(this);
            return providerFacade;
        }

        public ILoggerProvider AsLoggerProvider(LoggerProxyOptions options)
        {
            LoggerProviderProxy providerFacade = new LoggerProviderProxy(this, options);
            return providerFacade;
        }

        public ILoggerProvider AsLoggerProvider(Action<LoggerProxyOptions> config)
        {
            var options = GenerateLoggerOptions(config);

            LoggerProviderProxy providerFacade = new LoggerProviderProxy(this, options);
            return providerFacade;
        }

        public async Task PersistAsync()
        {
            LogLevel highestLevelRaised = LogLevel.Trace;

            if (EventHistory.Count > 0)
                highestLevelRaised = EventHistory.Max(x => x.GetLogLevel());

            foreach (ISink sink in Sinks)
            {
                if(highestLevelRaised >= sink.MinimumLevel || EventHistory.Count == 0)
                {
                    IEnumerable<IEvent> relevantEvtList = EventHistory ;
                    bool isExtended = (short)highestLevelRaised >= (short)sink.ExtendedLevel;
                    
                    // Filter only relevant events
                    if (!isExtended)
                        relevantEvtList = EventHistory.Where(x => x.GetLogLevel() >= sink.MinimumLevel);

                    // TODO: No es performannnnnnnte esta mierda, potencialmente repite el Prepare() si hay varios sinks
                    foreach (var evt in relevantEvtList)
                        evt.Prepare();

                    RequestInfo reqInfo = new RequestInfo(ApplicationInfoService, RequestInfoService, relevantEvtList);

                    await sink.PersistAsync(reqInfo);
                }
            }
        }

        public void Raise(IEvent evt)
        {
            if (evt == null) throw new ArgumentNullException(nameof(evt));

            EventHistory.Add(evt);
        }

        public void Raise<T>() where T : IEvent
        {
            try
            {
                IEvent evt = Activator.CreateInstance<T>();

                this.Raise(evt);
            }
            catch(MissingMethodException ex)
            {
                throw new RaiseEventWithoutParameterlessConstructorException();
            }
        }
    }
}
