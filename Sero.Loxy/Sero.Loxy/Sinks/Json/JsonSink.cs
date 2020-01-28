using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy.Sinks.Json
{
    public class JsonSink : ISink
    {
        private ILogger _logger;
        private JsonSinkBuilder _builder;

        public LogLevel ExtendedLevel => _builder.LevelExtended;
        public LogLevel MinimumLevel => _builder.LevelMinimum;

        public JsonSink(JsonSinkBuilder builder, ILogger logger)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (logger == null) throw new ArgumentNullException(nameof(logger), "A Microsoft.Extensions.Logging.ILogger<> instance must be provided.");

            _builder = builder;
            _logger = logger;
        }

        public async Task PersistAsync(IRequestInfo requestInfo)
        {
            var dto = new RequestInfoDTO(requestInfo);
            string jsonMessage = JsonConvert.SerializeObject(dto, _builder.JsonFormatting);
            _logger.Log(requestInfo.GetHighestLevel(), jsonMessage);
        }
    }
}
