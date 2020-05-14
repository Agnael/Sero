using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class CredentialRole : IApiResource
    {
        public string ApiResourceCode => SentinelResourceCodes.CredentialRoles;
        public string OwnerId { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
