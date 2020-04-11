using Sero.Core;
using System.Linq;

namespace Sero.Doorman
{
    public class AuthorizationService : IAuthorizationService
    {
        public readonly AuthorizationContext Context;

        public AuthorizationService(AuthorizationContext context)
        {
            this.Context = context;
        }

        public bool IsAuthorized(Endpoint endpoint)
        {
            var doormanAttr = endpoint.Action.GetAttribute<DoormanEndpointAttribute>();

            if(doormanAttr != null)
            {
                if (doormanAttr.LevelRequired == PermissionLevel.None)
                    return true;

                bool hasPermission =
                    Context.Permissions.Any(x =>
                        x.ResourceCode == endpoint.ResourceCode
                        && x.Level >= doormanAttr.LevelRequired);
                               
                if (hasPermission)
                    return true;
            }

            return false;
        }
    }
}
