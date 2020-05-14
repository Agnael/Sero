using Sero.Core;
using System.Linq;

namespace Sero.Gatekeeper
{
    public class AuthorizationService : IHateoasAuthorizator
    {
        public readonly AuthorizationContext Context;

        public AuthorizationService(AuthorizationContext context)
        {
            this.Context = context;
        }

        public bool IsAuthorized(IApiResource fromResource, Endpoint targetEndpoint)
        {
            var gatekeeperAttr = targetEndpoint.Action.GetAttribute<SafeEndpointAttribute>();

            if(gatekeeperAttr != null)
            {
                if (gatekeeperAttr.LevelRequired == PermissionLevel.None)
                    return true;

                var query = 
                    Context
                    .Permissions
                    .Where(x => 
                        x.ResourceCode == targetEndpoint.ResourceCode);

                query =
                    query
                    .Where(x => 
                        x.LevelOnAny >= gatekeeperAttr.LevelRequired
                        || (x.LevelOnOwned >= gatekeeperAttr.LevelRequired
                            && Context.User.OkAuthCredentialId == fromResource.OwnerId));


                bool hasPermission = query.Any();
                return hasPermission;
            }

            return false;
        }

        public bool IsAuthorized(Endpoint targetEndpoint)
        {
            var gatekeeperAttr = targetEndpoint.Action.GetAttribute<SafeEndpointAttribute>();

            if (gatekeeperAttr != null)
            {
                if (gatekeeperAttr.LevelRequired == PermissionLevel.None)
                    return true;

                bool hasPermission =
                    Context.Permissions.Any(x =>
                        x.ResourceCode == targetEndpoint.ResourceCode
                        && x.LevelOnOwned >= gatekeeperAttr.LevelRequired);

                if (hasPermission)
                    return true;
            }

            return false;
        }
    }
}
