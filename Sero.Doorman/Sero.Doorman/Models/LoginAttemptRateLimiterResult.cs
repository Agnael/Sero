using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class LoginAttemptRateLimiterResult
    {
        public bool IsApproved { get; set; }

        public TimeSpan? TimeToWait { get; set; }
        public string ErrorMessage { get; set; }
    }
}
