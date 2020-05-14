using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel
{
    public interface ILoginAttemptLimitingService
    {
        Task<LoginAttemptRateLimiterResult> Check(string fromIp, ILoginAttemptStore attemptStore);
    }
}
