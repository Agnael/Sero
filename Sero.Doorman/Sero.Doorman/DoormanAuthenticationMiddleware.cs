using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sero.Core.Middleware;
using Sero.Doorman.Events;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Middleware
{
    public class DoormanAuthenticationMiddleware : AbstractMiddleware
    {
        private DoormanOptions _options;
        private ILoxy _loxy;

        public DoormanAuthenticationMiddleware(RequestDelegate next) 
            : base(next)
        {

        }

        protected override async Task OnBefore(HttpContext context)
        {
            _loxy = this.GetService<ILoxy>(context);
            //var doormanOptionsMonitor = this.GetService<IOptionsMonitor<DoormanOptions>>(context);
            //var credentialStore = this.GetService<ICredentialStore>(context);

            //if (doormanOptionsMonitor == null)
            //    throw new UnconfiguredDoormanException();

            //if (context.Request.Headers.ContainsKey("Authorization"))
            //{
            //    _options = doormanOptionsMonitor.CurrentValue;

            //    string tokenString = context.Request.Headers["Authorization"];

            //    JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            //    TokenValidationParameters validationParameters = new TokenValidationParameters()
            //    {
            //        IssuerSigningKey = new SymmetricSecurityKey(_options.SigningKey),
            //        ValidateIssuerSigningKey = true,
            //        ValidateAudience = false,
            //        ValidateIssuer = false,
            //        ValidateLifetime = false,
            //        ValidateActor = false
            //    };

            //    SecurityToken token = null;
            //    ClaimsPrincipal principal = null;

            //    try
            //    {
            //        principal = jwtHandler.ValidateToken(tokenString, validationParameters, out token);

            //        var identity = principal.Identity as ClaimsIdentity;

            //        // TODO: RECUPERAR LOS ROLES DEL USUARIO DESDE EL STORE Y AGREGARLOS COMO CLAIMS EN LA IDENTITY ACTUAL
            //        Claim nameIdentifierClaim = identity.Claims.First(x => x.Type.Equals(ClaimTypes.NameIdentifier));
            //        int idUserCurrent = int.Parse(nameIdentifierClaim.Value); ;

            //        Credential currentUser = await credentialStore.FetchAsync(idUserCurrent);

            //        if(currentUser == null)
            //        {
            //            _loxy.Raise<JwtWithUnexistingIdDetected>();
            //        }
            //        else
            //        {
            //            foreach (Permission permission in currentUser.Permissions)
            //                identity.AddClaim(new Claim(Constants.ClaimTypes.Permission, permission.Name));

            //            await context.SignInAsync(principal);
            //            _loxy.Raise(new UserDetected(principal));
            //        }
            //    }
            //    catch(Exception ex)
            //    {
            //        _loxy.Raise(new CorruptJwtDetected(ex));
            //    }
            //}
            //else
            //{
            //    _loxy.Raise(new UnknownGuestDetected());
            //}
        }

        protected override async Task OnAfter(HttpContext context)
        {
            //var user = context.User;

            //if (user.Identity.IsAuthenticated)
            //{
            //    JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            //    SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor();

            //    ClaimsIdentity newIdentity = new ClaimsIdentity();
            //    newIdentity.AddClaim(user.Claims.First(x => x.Type.Equals(ClaimTypes.NameIdentifier)));

            //    descriptor.Subject = newIdentity;
            //    descriptor.Issuer = _options.Issuer;
            //    descriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_options.SigningKey), SecurityAlgorithms.HmacSha256);

            //    SecurityToken token = jwtHandler.CreateToken(descriptor);

            //    string tokenString = jwtHandler.WriteToken(token);

            //    context.Response.Headers.Add("Authorization", string.Format("bearer {0}", tokenString));

            //    _loxy.Raise(new JwtSent(tokenString));
            //}

            //_loxy.Raise<NoJwtSent>();
        }

        protected override async Task<bool> OnErrorShouldRethrow(HttpContext context, Exception ex)
        {
            // TODO: Cambiar para que si es una "DoormanException" (crear esa clase base), entonces no se rethrowea porque se supone
            // que aca sabe handlerearla, cualquier otra exception si la rethrowea porque no es asunto nuestro.

            //_loxy.Raise(new DoormanError(ex));
            
            return true;
        }
    }
}
