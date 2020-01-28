using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using Nito.AsyncEx;
using Microsoft.AspNetCore.Mvc;

namespace Sero.Doorman
{
    public class DoormanAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

            //CheckAccessAttribute requirePermissionAttribute = 
            //    (CheckAccessAttribute)descriptor.EndpointMetadata.FirstOrDefault(x => x is CheckAccessAttribute);

            //if (requirePermissionAttribute == null)
            //    throw new UnsecuredActionException();

            //string requiredPermissionName = requirePermissionAttribute.ResourceCode;
            //var user = context.HttpContext.User;

            //// TODO: Oleg esta mierda de ForbidResult está muy atado al AddAuthorization de aspnetcore, que no quiero usar, asique ponete a hacer estos IActionResults a mano para que no sea tan asqueroso esto.
            //if (!user.Identity.IsAuthenticated)
            //    context.Result = new ForbidResult();

            //bool isAllowed = user.HasClaim(Constants.ClaimTypes.Permission, requiredPermissionName);

            //// The current user doesn't have the permission the resource requires
            //if (!isAllowed)
            //    context.Result = new UnauthorizedResult();
            
        }
    }
}
