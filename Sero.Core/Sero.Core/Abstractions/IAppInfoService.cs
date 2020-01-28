using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IAppInfoService
    {
        string AppEnvironment { get; }
        string AppName { get; }
        string AppVersion { get; }
        string MachineName { get; }
        string LocalIp { get; }
    }
}
