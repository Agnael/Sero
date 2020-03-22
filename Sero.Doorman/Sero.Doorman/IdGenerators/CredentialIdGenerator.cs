using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public static class CredentialIdGenerator
    {
        public static string Generate()
        {
            string newId = shortid.ShortId.Generate(true, true, 8);
            return newId;
        }
    }
}
