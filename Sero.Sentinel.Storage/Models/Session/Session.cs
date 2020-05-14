using Sero.Core;
using System;

namespace Sero.Sentinel.Storage
{
    public class Session : IApiResource
    {
        public string ApiResourceCode => SentinelResourceCodes.Sessions;

        public string OwnerId { get; set; }
        public string CredentialId { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public bool AllowSelfRenewal { get; set; }

        public UserDevice Device { get; set; }
        public UserAgent Agent { get; set; }

        public Session()
        {

        }
    }
}
