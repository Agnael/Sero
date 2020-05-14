using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IRequestInfoService
    {
        string Url { get; }
        string Verb { get; }
        string QueryString { get; }
        string RequestBody { get; }
        string AcceptLanguageHeader { get; }
        UserAgentOverview UserAgent { get; }
        string RequestId { get; }
        DateTime StartDateUtc { get; }
        long StartUnixTimestamp { get; }
        int IdProcess { get; }
        string RemoteIp { get; }
    }
}
