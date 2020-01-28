using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Sinks.Json
{
    [JsonObject]
    internal class RequestInfoDTO : IRequestInfo
    {
        public string Level { get; set; }
        public string Environment { get; set; }
        public string MachineName { get; set; }
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string Verb { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        public string AcceptLanguage { get; set; }
        public string RequestTraceIdentitfier { get; set; }
        public DateTime Datetime { get; set; }
        public long UnixTimestamp { get; set; }
        public int IdProcess { get; set; }
        public string LocalIpAddress { get; set; }
        public string RemoteIpAddress { get; set; }
        public IEnumerable<IEvent> Events { get; set; }

        public RequestInfoDTO() { }

        public RequestInfoDTO(IRequestInfo req)
        {
            this.MachineName = req.MachineName;
            this.RequestBody = req.RequestBody;
            this.AppName = req.AppName;
            this.AppVersion = req.AppVersion;
            this.Environment = req.Environment;
            this.Level = req.Level;
            this.Verb = req.Verb;
            this.Url = req.Url;
            this.QueryString = req.QueryString;
            this.AcceptLanguage = req.AcceptLanguage;
            this.RequestTraceIdentitfier = req.RequestTraceIdentitfier;
            this.Datetime = req.Datetime;
            this.IdProcess = req.IdProcess;
            this.LocalIpAddress = req.LocalIpAddress;
            this.RemoteIpAddress = req.RemoteIpAddress;

            if(req.Events != null)
            {
                var eventList = new List<IEvent>();
                foreach (var evt in req.Events)
                {
                    var eventDto = new EventDTO(evt);
                    eventList.Add(eventDto);
                }

                this.Events = eventList;
            }
        }

        public LogLevel GetHighestLevel()
        {
            throw new NotImplementedException();
        }
    }
}
