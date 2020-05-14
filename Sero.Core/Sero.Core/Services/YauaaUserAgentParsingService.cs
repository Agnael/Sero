using OrbintSoft.Yauaa;
using OrbintSoft.Yauaa.Analyzer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    /// <summary>
    ///     User-Agent header value analyzer based on the YAUAA library: 
    ///     https://github.com/OrbintSoft/yauaa.netstandard
    /// </summary>
    public class YauaaUserAgentParsingService : IUserAgentParsingService
    {
        private readonly UserAgentAnalyzer _analyzer;

        public YauaaUserAgentParsingService()
        {
            _analyzer =
                UserAgentAnalyzer
                .NewBuilder()
                .HideMatcherLoadStats()
                .WithField(OrbintSoft.Yauaa.Analyzer.UserAgent.AGENT_NAME)
                .WithField(OrbintSoft.Yauaa.Analyzer.UserAgent.AGENT_VERSION)
                .WithField(OrbintSoft.Yauaa.Analyzer.UserAgent.DEVICE_CLASS)
                .WithField(OrbintSoft.Yauaa.Analyzer.UserAgent.DEVICE_NAME)
                .WithCache(10000)
                .Build();
        }

        public UserAgentOverview Parse(string userAgentHeaderValue)
        {
            OrbintSoft.Yauaa.Analyzer.UserAgent userAgent = _analyzer.Parse(userAgentHeaderValue);

            UserAgentOverview dto = new UserAgentOverview();

            dto.AgentName = userAgent.GetValue(OrbintSoft.Yauaa.Analyzer.UserAgent.AGENT_NAME);
            dto.AgentVersion = userAgent.GetValue(OrbintSoft.Yauaa.Analyzer.UserAgent.AGENT_VERSION);
            dto.DeviceClass = userAgent.GetValue(OrbintSoft.Yauaa.Analyzer.UserAgent.DEVICE_CLASS);
            dto.DeviceName = userAgent.GetValue(OrbintSoft.Yauaa.Analyzer.UserAgent.DEVICE_NAME);

            return dto;
        }
    }
}
