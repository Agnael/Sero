using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ILoginAttemptLimitingStrategy
    {
        Task<LoginAttemptRateLimiterResult> Check(string fromIp, ILoginAttemptStore attemptStore);
    }
}
