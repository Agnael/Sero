﻿using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class LoginAttempt
    {
        public DateTime AttemptDate { get; set; }
        public string FromIp { get; set; }

        public LoginAttempt()
        {

        }

        public LoginAttempt(string fromIp, DateTime attemptDt)
        {
            this.AttemptDate = attemptDt;
            this.FromIp = fromIp;
        }
    }
}
