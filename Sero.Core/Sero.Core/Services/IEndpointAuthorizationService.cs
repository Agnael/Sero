using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IEndpointAuthorizationService
    {
        bool IsAuthorized(Endpoint endpoint);
    }
}
