using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Sinks.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public static class SinkManagerExtensions
    {
        private const string ERROR_LOGGER_NULL = "An ILogger instance must be registered in the DI Container, or provided explicitly as a parameter.";

        public static JsonSinkBuilder AddJsonSink(this SinkManager manager)
        {
            var serviceProvider = manager.CurrentLoxyBuilder.ServiceCollection.BuildServiceProvider();
            ILogger logger = serviceProvider.GetService<ILogger<JsonSink>>();

            return manager.AddJsonSink(logger);
        }

        public static JsonSinkBuilder AddJsonSink(this SinkManager manager, ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger), ERROR_LOGGER_NULL);

            var builder = new JsonSinkBuilder(manager);
            var sink = new JsonSink(builder, logger);

            manager.AddSink(sink);

            return builder;
        }
    }
}
