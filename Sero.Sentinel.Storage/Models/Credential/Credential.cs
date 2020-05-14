using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel.Storage
{
    public class Credential : IApiResource
    {
        public string ApiResourceCode => SentinelResourceCodes.Credentials;

        public string OwnerId { get; set; }
        public string CredentialId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime BirthDate { get; set; }

        public List<CredentialRole> Roles { get; set; }
        public List<Session> Sessions { get; set; }

        public Credential() 
        {
            Roles = new List<CredentialRole>();
            Sessions = new List<Session>();
        }
    }
}
