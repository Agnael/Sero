using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class ExceptionInfo
    {
        [JsonProperty(Order = 1)]
        public string ExceptionClass { get; set; }

        [JsonProperty(Order = 2)]
        public string Message { get; set; }

        [JsonProperty(Order = 3)]
        public string[] StackTrace { get; set; }
    }
}
