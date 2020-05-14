using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public enum SessionSorting
    {
        DeviceName,
        DeviceType,
        AgentNameAndVersion,
        LastActiveDate,
        LoginDate,
        ExpirationDate,
        CredentialId
    }
}
