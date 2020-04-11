using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class SessionsController : BaseHateoasController
    {
        public readonly ICredentialStore CredentialStore;
        public readonly ISessionStore SessionStore;
        public readonly SessionService SessionService;
        public readonly ILoginAttemptStore LoginAttemptStore;
        public readonly IRequestInfo RequestInfo;
        public readonly ILoginAttemptLimitingStrategy LoginAttemptRateLimiter;
        public readonly ICredentialPenaltyStore CredentialPenaltyStore;
        public readonly AuthorizationContext AuthorizationContext;

        public SessionsController(
            ICredentialStore credentialStore,
            ISessionStore sessionStore,
            SessionService sessionService,
            ILoginAttemptStore loginAttemptStore,
            IRequestInfo reqInfo,
            ILoginAttemptLimitingStrategy loginAttemptRateLimiter,
            ICredentialPenaltyStore credentialPenaltyStore,
            AuthorizationContext authorizationContext)
        {
            this.CredentialStore = credentialStore;
            this.SessionStore = sessionStore;
            this.SessionService = sessionService;
            this.LoginAttemptStore = loginAttemptStore;
            this.RequestInfo = reqInfo;
            this.LoginAttemptRateLimiter = loginAttemptRateLimiter;
            this.CredentialPenaltyStore = credentialPenaltyStore;
            this.AuthorizationContext = authorizationContext;
        }

        [HttpGet("api/doorman/sessions/{credentialId}")]
        [Getter("Session")]
        [DoormanEndpoint(Constants.ResourceCodes.Sessions, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCredentialId([GetterParameter][FromRoute] string credentialId)
        {
            if (string.IsNullOrEmpty(credentialId))
                throw new ArgumentNullException(nameof(credentialId));

            Session session = await SessionStore.Get(credentialId);

            if (session == null)
                return NotFound();

            return Element<Session>(session);
        }
        
        [HttpGet("api/doorman/sessions/{credentialId}/credential")]
        [DoormanEndpoint(Constants.ResourceCodes.Sessions, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetSessionCredential([FromRoute] string credentialId)
        {
            if (string.IsNullOrEmpty(credentialId))
                throw new ArgumentNullException(nameof(credentialId));

            Session session = await SessionStore.Get(credentialId);

            if (session == null)
                return NotFound();

            Credential credential = await CredentialStore.Get(credentialId);

            return Element<Credential>(credential);
        }

        [HttpPost("api/doorman/auth/login")]
        [DoormanEndpoint(Constants.ResourceCodes.Sessions, PermissionLevel.None, EndpointScope.Collection)]
        public async Task<IActionResult> Login([FromBody] LoginForm form)
        {
            // El form está comlpeto? --------------------
            if (form == null)
                return new BadRequestResult();

            if(string.IsNullOrEmpty(form.Email))
                ModelState.AddModelError(nameof(form.Email), "Required");

            if(string.IsNullOrEmpty(form.Password))
                ModelState.AddModelError(nameof(form.Password), "Required");

            if (!ModelState.IsValid)
                return ValidationError();

            // La IP tiene permiso de intentar login? --------------------
            var attemptRateResult = await LoginAttemptRateLimiter.Check(RequestInfo.RemoteIpAddress, LoginAttemptStore);
            if(!attemptRateResult.IsApproved)
            {
                ModelState.AddModelError("", attemptRateResult.ErrorMessage);
                return ValidationError();
            }

            LoginAttempt attempt = new LoginAttempt(this.RequestInfo.RemoteIpAddress, DateTime.UtcNow);

            // La credencial existe? --------------------
            string failedLoginMsg = "Invalid email and password combination.";
            Credential credential = await CredentialStore.GetByEmail(form.Email);
            if (credential == null)
            {
                ModelState.AddModelError("", failedLoginMsg);
                await LoginAttemptStore.Create(attempt);
                return ValidationError();
            }

            string newCalculatedHash = HashingUtil.GenerateHash(form.Password, credential.PasswordSalt);
            if (newCalculatedHash != credential.PasswordHash)
            {
                ModelState.AddModelError("", failedLoginMsg);
                await LoginAttemptStore.Create(attempt);
                return ValidationError();
            }

            CredentialPenalty activePenalty = await CredentialPenaltyStore.Get(credential.CredentialId, DateTime.UtcNow);
            if(activePenalty != null)
            {
                string validationMsg = null;

                if (activePenalty.EndDate.HasValue)
                    validationMsg = string.Format("User temporarily banned, until [{0}]. Reason: '{1}'", activePenalty.EndDate.Value.ToString(), activePenalty.Reason);
                else
                    validationMsg = string.Format("User permanently banned. Reason: '{0}'", activePenalty.Reason);

                ModelState.AddModelError("", validationMsg);
                await LoginAttemptStore.Create(attempt);
                return ValidationError();
            }

            // Crea la sesión
            Session newSession = new Session();
            newSession.CredentialId = credential.CredentialId;
            newSession.LoginDate = DateTime.UtcNow;
            newSession.ExpirationDate = DateTime.UtcNow.AddDays(1);
            newSession.LastActiveDate = newSession.LoginDate;
            newSession.AllowSelfRenewal = form.RememberMe;

            await SessionStore.Create(newSession);
            this.AuthorizationContext.SetSession(newSession);

            ClaimsIdentity newIdentity = new ClaimsIdentity("Basic");
            newIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, credential.CredentialId));

            ClaimsPrincipal newPrincipal = new ClaimsPrincipal(newIdentity);
            
            this.SignIn(newPrincipal, "Bearer");
            return Ok();
        }
    }
}
