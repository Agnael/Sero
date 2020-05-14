using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Sero.Gatekeeper
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetCredentialId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            string credentialId = claim.Value;
            return credentialId;
        }
    }
}
