using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DummyAuthorizationService : IAuthorizationService
    {
        public bool IsAuthorized(Endpoint endpoint)
        {
            return true;
        }
    }
}
