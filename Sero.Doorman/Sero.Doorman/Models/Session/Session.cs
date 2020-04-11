using Sero.Core;
using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Doorman
{
    public class Session : Element
    {
        public string CredentialId { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime LastActiveDate { get; set; }
        public bool AllowSelfRenewal { get; set; }

        public Session()
            : base(Constants.ResourceCodes.Sessions)
        {

        }
    }
}
