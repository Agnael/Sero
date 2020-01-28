using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Sero.Core.Services
{
    public class AppInfoService : IAppInfoService
    {
        IHostingEnvironment _hostingEnvironment;

        private readonly string _appEnvironment;
        private readonly string _appName;
        private readonly string _appVersion;
        private readonly string _machineName;

        public AppInfoService(IHostingEnvironment env)
        {
            if (env == null) throw new ArgumentNullException(nameof(env));
            _hostingEnvironment = env;

            _appEnvironment = _hostingEnvironment.EnvironmentName;
            _appName = Assembly.GetEntryAssembly().GetName().Name;

            Version v = Assembly.GetEntryAssembly().GetName().Version;
            _appVersion = string.Format(CultureInfo.InvariantCulture,
                                        @"{0}.{1}.{2} (r{3})",
                                        v.Major,
                                        v.Minor,
                                        v.Build,
                                        v.Revision);

            _machineName = Environment.MachineName;
        }

        string IAppInfoService.AppEnvironment => _appEnvironment;

        string IAppInfoService.AppName => _appName;

        string IAppInfoService.AppVersion => _appVersion;

        string IAppInfoService.MachineName => _machineName;

        /// <summary>
        ///     In case IP changes between calls, it gets evaluated each time.
        /// </summary>
        string IAppInfoService.LocalIp
        {
            get
            {                
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }

                return Constants.UNKNOWN_IP;
            }
        }
    }
}
