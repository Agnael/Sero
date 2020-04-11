using Sero.Core;
using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Doorman
{
    public class CredentialPenalty
    {
        public string CredentialId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Reason { get; set; }

        public CredentialPenalty()
        {

        }

        public CredentialPenalty(string credentialId, string reason, DateTime start, DateTime? end = null)
        {
            this.CredentialId = credentialId;
            this.StartDate = start;
            this.EndDate = end;
            this.Reason = reason;
        }
    }
}
