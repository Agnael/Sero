using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sero.Loxy.Events
{
    public class JsonStateEvent<TState> : AbstractEvent<TState>
    {
        public JsonStateEvent(LogLevel level, string category, string message, TState state) 
            : base(level, category, message, state)
        {
        }

        protected override IEnumerable<string> FormatState(TState state)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                writer.QuoteChar = '\'';

                JsonSerializer ser = new JsonSerializer();
                ser.Serialize(writer, state);
            }

            string json = sb.ToString();
            return new string[] { json };
        }
    }
}
