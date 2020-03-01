using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class DoormanOptions
    {
        public string Issuer { get; private set; }

        public string SigningKeyBase64SourceString { get; private set; }
        public byte[] SigningKey { get; private set; }

        public DoormanOptions()
        {
            WithIssuer("Sero.Doorman");
            WithSecurityKey("j3Kkms555Msmkekwau3IsSJJJSKA3geLOa3AgnAEL83a");
        }

        public DoormanOptions WithSecurityKey(string base64string)
        {
            SigningKeyBase64SourceString = base64string;
            SigningKey = Convert.FromBase64String(base64string);
            return this;
        }

        public DoormanOptions WithIssuer(string issuer)
        {
            Issuer = issuer;
            return this;
        }
    }
}
