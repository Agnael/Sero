using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string SigningSecret { get; set; }

        private byte[] _signingSecretBytes;
        public byte[] SigningSecretBytes
        {
            get
            {
                if (_signingSecretBytes == null)
                    _signingSecretBytes = Encoding.UTF8.GetBytes(SigningSecret);

                return _signingSecretBytes;
            } 
            private set
            {
                _signingSecretBytes = value;
            }
        }
        
        public JwtConfiguration()
        {

        }
    }
}
