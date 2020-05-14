using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Test;

namespace Sero.Sentinel
{
    [AllowAnonymous]
    public class SessionsController : BaseHateoasController
    {
        public readonly ICredentialStore CredentialStore;
        public readonly ISessionStore SessionStore;
        public readonly ILoginAttemptStore LoginAttemptStore;
        public readonly IRequestInfoService RequestInfoService;
        public readonly ILoginAttemptLimitingService LoginAttemptLimitingService;
        public readonly ICredentialPenaltyStore CredentialPenaltyStore;

        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public SessionsController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            TestUserStore users,

            ICredentialStore credentialStore,
            ISessionStore sessionStore,
            ILoginAttemptStore loginAttemptStore,
            IRequestInfoService requestInfoService,
            ILoginAttemptLimitingService loginAttemptLimitingService,
            ICredentialPenaltyStore credentialPenaltyStore)
        {
            // if the TestUserStore is not in DI, then we'll just use the global users collection
            // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
            _users = users;

            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;

            this.CredentialStore = credentialStore;
            this.SessionStore = sessionStore;
            this.LoginAttemptStore = loginAttemptStore;
            this.RequestInfoService = requestInfoService;
            this.LoginAttemptLimitingService = loginAttemptLimitingService;
            this.CredentialPenaltyStore = credentialPenaltyStore;
        }

        [HttpGet("api/gatekeeper/credentials/{credentialId}/session")]
        [Getter("Session")]
        [SafeEndpoint(SentinelResourceCodes.Sessions, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCredentialId([GetterParameter][FromRoute] string credentialId)
        {
            if (string.IsNullOrEmpty(credentialId))
                throw new ArgumentNullException(nameof(credentialId));

            UserAgentOverview agent = RequestInfoService.UserAgent;

            Session session = 
                await SessionStore.Get(credentialId, agent.DeviceClass, agent.DeviceName, agent.AgentName, agent.AgentVersion);

            if (session == null)
                return NotFound();

            return Element<Session>(session);
        }

        [HttpPost]
        [SafeEndpoint(SentinelResourceCodes.Sessions, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> CancelLogin(LoginCancellationForm form)
        {
            var context = await _interaction.GetAuthorizationContextAsync(form.ReturnUrl);

            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (await _clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // If the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return Redirect(string.Format("/redirection?ReturnUrl={0}", form.ReturnUrl));
                }

                return Redirect(form.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost("api/sentinel/sessions")]
        [SafeEndpoint(SentinelResourceCodes.Sessions, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> Login([FromBody] SessionCreateForm form)
        {
            // El form está comlpeto? --------------------
            if (form == null)
                return new BadRequestResult();

            if (string.IsNullOrEmpty(form.UsernameOrEmail))
                ModelState.AddModelError(nameof(form.UsernameOrEmail), "Required");

            if (string.IsNullOrEmpty(form.Password))
                ModelState.AddModelError(nameof(form.Password), "Required");

            if (!ModelState.IsValid)
                return ValidationError();

            // La IP tiene permiso de intentar login? --------------------
            var attemptRateResult = await LoginAttemptLimitingService.Check(RequestInfoService.RemoteIp, LoginAttemptStore);
            if (!attemptRateResult.IsApproved)
            {
                ModelState.AddModelError("", attemptRateResult.ErrorMessage);
                return ValidationError();
            }

            LoginAttempt attempt = new LoginAttempt(this.RequestInfoService.RemoteIp, DateTime.UtcNow);

            // La credencial existe? --------------------
            string failedLoginMsg = "Invalid email and password combination.";

            Credential credential = null;
            bool isEmail = form.UsernameOrEmail.IsEmail();

            if(isEmail)
                credential = await CredentialStore.GetByEmail(form.UsernameOrEmail);
            else
                credential = await CredentialStore.Get(form.UsernameOrEmail);


            if (credential == null)
            {
                ModelState.AddModelError("", failedLoginMsg);
                await LoginAttemptStore.Create(attempt);
                return ValidationError();
            }

            // La contraseña es correcta?
            string newCalculatedHash = HashingUtil.GenerateHash(form.Password, credential.PasswordSalt);
            if (newCalculatedHash != credential.PasswordHash)
            {
                ModelState.AddModelError("", failedLoginMsg);
                await LoginAttemptStore.Create(attempt);
                return ValidationError();
            }

            // El usuario está penalizado?
            CredentialPenalty activePenalty = await CredentialPenaltyStore.Get(credential.CredentialId, DateTime.UtcNow);
            if (activePenalty != null)
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

            var agent = RequestInfoService.UserAgent;

            // La credencial ya tiene una sesión activa?
            Session session = 
                await this.SessionStore.Get(
                    credential.CredentialId, 
                    agent.DeviceClass, 
                    agent.DeviceName, 
                    agent.AgentName, 
                    agent.AgentVersion);

            if (session != null)
            {
                session.LastActiveDate = DateTime.UtcNow;

                if (session.AllowSelfRenewal)
                    session.ExpirationDate = session.LastActiveDate.AddDays(1);

                await SessionStore.Update(session);
            }
            else
            {
                // Crea la sesión
                session = new Session();
                session.CredentialId = credential.CredentialId;
                session.LoginDate = DateTime.UtcNow;
                session.ExpirationDate = DateTime.UtcNow.AddDays(1);
                session.LastActiveDate = session.LoginDate;
                session.AllowSelfRenewal = form.IsRememberLogin;
                session.Device = new UserDevice(agent.DeviceClass, agent.DeviceName);
                session.Agent = new UserAgent(agent.AgentName, agent.AgentVersion);

                await SessionStore.Create(session);
            }

            // Autentifica
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(form.ReturnUrl);

            await _events.RaiseAsync(new UserLoginSuccessEvent(credential.DisplayName, credential.CredentialId, credential.DisplayName, clientId: context?.ClientId));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (form.IsRememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(8))
                };
            };

            // issue authentication cookie with subject ID and username
            var isuser = new IdentityServerUser(credential.CredentialId)
            {
                DisplayName = credential.DisplayName
            };

            await HttpContext.SignInAsync(isuser, props);

            // Devuelve el recurso Session
            return Element<Session>(session);
        }
    }
}
