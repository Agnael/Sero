using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Sinks.Json
{
    [JsonObject]
    internal class EventDTO : IEvent
    {
        [JsonProperty]
        public string Level { get; set; }

        [JsonProperty]
        public string Category { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public DateTime DateTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ExceptionInfo> Exception { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> Details { get; set; }

        private LogLevel _level;

        public EventDTO() { }

        public EventDTO(IEvent evt)
        {
            Level = evt.Level;
            Category = evt.Category;
            Type = evt.Type;
            Message = evt.Message;
            Exception = evt.Exception;
            Details = evt.Details;
            _level = evt.GetLogLevel();
            DateTime = evt.DateTime;
        }

        public LogLevel GetLogLevel()
        {
            return _level;
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
