using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DummyAuthorizationService : IHateoasAuthorizator
    {
        public bool IsAuthorized(IApiResource fromResource, Endpoint targetEndpoint)
        {
            return true;
        }

        public bool IsAuthorized(Endpoint targetEndpoint)
        {
            return true;
        }
    }
}
