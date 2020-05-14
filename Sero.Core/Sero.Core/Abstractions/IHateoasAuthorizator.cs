using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IHateoasAuthorizator
    {
        bool IsAuthorized(IApiResource fromResource, Endpoint targetEndpoint);
        bool IsAuthorized(Endpoint targetEndpoint);
    }
}
