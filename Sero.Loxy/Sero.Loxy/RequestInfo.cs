using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Core;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class RequestInfo : IRequestInfo
    {
        public string MachineName { get; private set; }
        public string AppName { get; private set; }
        public string AppVersion { get; private set; }
        public string Environment { get; private set; }
        public string Level { get; private set; }
        public string Verb { get; private set; }
        public string Url { get; private set; }
        public string QueryString { get; private set; }
        public string RequestBody { get; private set; }
        public string AcceptLanguage { get; private set; }
        public string CultureCodeSelected { get; private set; }
        public string RequestTraceIdentitfier { get; private set; }
        public DateTime Datetime { get; private set; }
        public int IdProcess { get; private set; }
        public string LocalIpAddress { get; private set; }
        public string RemoteIpAddress { get; private set; }

        public IEnumerable<IEvent> Events { get; private set; }

        private LogLevel _highestLogLevel;

        public RequestInfo(IAppInfoService app,
                            IRequestInfoService req,
                            IEnumerable<IEvent> eventList)
        {
            this.MachineName = app.MachineName;
            this.AppName = app.AppName;
            this.AppVersion = app.AppVersion;
            this.Environment = app.AppEnvironment;

            this.Url = req.Url;
            this.QueryString = req.QueryString;
            this.RequestBody = req.RequestBody;
            this.Verb = req.Verb;

            this.AcceptLanguage = req.AcceptLanguageHeader;
            this.RequestTraceIdentitfier = req.RequestId;

            this.Datetime = req.StartDateUtc;
            this.IdProcess = req.IdProcess;

            this.LocalIpAddress = app.LocalIp;
            this.RemoteIpAddress = req.RemoteIp;
            this.Events = eventList;

            this.Level = "UNDEFINED";

            if (this.Events != null && this.Events.Count() > 0)
            {
                _highestLogLevel = this.Events.Max(x => x.GetLogLevel());
                this.Level = _highestLogLevel.ToString();
            }
        }

        public LogLevel GetHighestLevel()
        {
            return _highestLogLevel;
        }
    }
}
