using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sero.Core;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sero.Sentinel
{
    //[ApiController]
    //public class SessionsController : BaseHateoasController
    //{
    //    public readonly ICredentialStore CredentialStore;
    //    public readonly ISessionStore SessionStore;
    //    public readonly SessionService SessionService;
    //    public readonly ILoginAttemptStore LoginAttemptStore;
    //    public readonly IRequestInfoService RequestInfo;
    //    public readonly ILoginAttemptLimitingStrategy LoginAttemptRateLimiter;
    //    public readonly ICredentialPenaltyStore CredentialPenaltyStore;
    //    public readonly AuthorizationContext AuthorizationContext;
    //    public readonly GtkOptions GatekeeperOptions;

    //    public SessionsController(
    //        ICredentialStore credentialStore,
    //        ISessionStore sessionStore,
    //        //SessionService sessionService,
    //        ILoginAttemptStore loginAttemptStore,
    //        IRequestInfoService reqInfo,
    //        ILoginAttemptLimitingStrategy loginAttemptRateLimiter,
    //        ICredentialPenaltyStore credentialPenaltyStore,
    //        AuthorizationContext authorizationContext,
    //        IOptionsMonitor<GtkOptions> gatekeeperOptions)
    //    {
    //        this.CredentialStore = credentialStore;
    //        this.SessionStore = sessionStore;
    //        //this.SessionService = sessionService;
    //        this.LoginAttemptStore = loginAttemptStore;
    //        this.RequestInfo = reqInfo;
    //        this.LoginAttemptRateLimiter = loginAttemptRateLimiter;
    //        this.CredentialPenaltyStore = credentialPenaltyStore;
    //        this.AuthorizationContext = authorizationContext;
    //        this.GatekeeperOptions = gatekeeperOptions.CurrentValue;
    //    }

    //    [HttpGet("api/gatekeeper/sessions/{credentialId}")]
    //    [Getter("Session")]
    //    [SafeEndpoint(GtkResourceCodes.Sessions, PermissionLevel.Read, EndpointScope.Element)]
    //    public async Task<IActionResult> GetByCredentialId([GetterParameter][FromRoute] string credentialId)
    //    {
    //        if (string.IsNullOrEmpty(credentialId))
    //            throw new ArgumentNullException(nameof(credentialId));

    //        Session session = await SessionStore.Get(credentialId);

    //        if (session == null)
    //            return NotFound();

    //        return Element<Session>(session);
    //    }
        
    //    [HttpGet("api/gatekeeper/sessions/{credentialId}/credential")]
    //    [SafeEndpoint(GtkResourceCodes.Sessions, PermissionLevel.Read, EndpointScope.Element)]
    //    public async Task<IActionResult> GetSessionCredential([FromRoute] string credentialId)
    //    {
    //        if (string.IsNullOrEmpty(credentialId))
    //            throw new ArgumentNullException(nameof(credentialId));

    //        Session session = await SessionStore.Get(credentialId);

    //        if (session == null)
    //            return NotFound();
            
    //        Credential credential = await CredentialStore.Get(credentialId);
    //        CredentialVM vm = new CredentialVM(credential);
    //        return Element<Credential>(vm);
    //    }

    //    [HttpPost("api/gatekeeper/sessions")]
    //    [SafeEndpoint(GtkResourceCodes.Sessions, PermissionLevel.None, EndpointScope.Collection)]
    //    public async Task<IActionResult> Login([FromBody] LoginForm form)
    //    {
    //        // El form está comlpeto? --------------------
    //        if (form == null)
    //            return new BadRequestResult();

    //        if(string.IsNullOrEmpty(form.Email))
    //            ModelState.AddModelError(nameof(form.Email), "Required");

    //        if(string.IsNullOrEmpty(form.Password))
    //            ModelState.AddModelError(nameof(form.Password), "Required");

    //        if (!ModelState.IsValid)
    //            return ValidationError();

    //        // La IP tiene permiso de intentar login? --------------------
    //        var attemptRateResult = await LoginAttemptRateLimiter.Check(RequestInfo.RemoteIp, LoginAttemptStore);
    //        if(!attemptRateResult.IsApproved)
    //        {
    //            ModelState.AddModelError("", attemptRateResult.ErrorMessage);
    //            return ValidationError();
    //        }

    //        LoginAttempt attempt = new LoginAttempt(this.RequestInfo.RemoteIp, DateTime.UtcNow);

    //        // La credencial existe? --------------------
    //        string failedLoginMsg = "Invalid email and password combination.";
    //        Credential credential = await CredentialStore.GetByEmail(form.Email);
    //        if (credential == null)
    //        {
    //            ModelState.AddModelError("", failedLoginMsg);
    //            await LoginAttemptStore.Create(attempt);
    //            return ValidationError();
    //        }

    //        // La contraseña es correcta?
    //        string newCalculatedHash = HashingUtil.GenerateHash(form.Password, credential.PasswordSalt);
    //        if (newCalculatedHash != credential.PasswordHash)
    //        {
    //            ModelState.AddModelError("", failedLoginMsg);
    //            await LoginAttemptStore.Create(attempt);
    //            return ValidationError();
    //        }

    //        // El usuario está penalizado?
    //        CredentialPenalty activePenalty = await CredentialPenaltyStore.Get(credential.CredentialId, DateTime.UtcNow);
    //        if(activePenalty != null)
    //        {
    //            string validationMsg = null;

    //            if (activePenalty.EndDate.HasValue)
    //                validationMsg = string.Format("User temporarily banned, until [{0}]. Reason: '{1}'", activePenalty.EndDate.Value.ToString(), activePenalty.Reason);
    //            else
    //                validationMsg = string.Format("User permanently banned. Reason: '{0}'", activePenalty.Reason);

    //            ModelState.AddModelError("", validationMsg);
    //            await LoginAttemptStore.Create(attempt);
    //            return ValidationError();
    //        }
                       
    //        // La credencial ya tiene una sesión activa?
    //        Session session = await this.SessionStore.Get(credential.CredentialId);
    //        if (session != null)
    //        {
    //            session.LastActiveDate = DateTime.UtcNow;

    //            if (session.AllowSelfRenewal)
    //                session.ExpirationDate = session.LastActiveDate.AddDays(1);

    //            await SessionStore.Update(session);
    //        }
    //        else
    //        {
    //            // Crea la sesión
    //            session = new Session();
    //            session.CredentialId = credential.CredentialId;
    //            session.LoginDate = DateTime.UtcNow;
    //            session.ExpirationDate = DateTime.UtcNow.AddDays(1);
    //            session.LastActiveDate = session.LoginDate;
    //            session.AllowSelfRenewal = form.RememberMe;

    //            await SessionStore.Create(session);
    //        }

    //        ClaimsIdentity newIdentity = new ClaimsIdentity("Basic");
    //        newIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, credential.CredentialId));

    //        ClaimsPrincipal newPrincipal = new ClaimsPrincipal(newIdentity);
            
    //        HttpContext.User = newPrincipal;
    //        this.AuthorizationContext.SetSession(session);

    //        // Crea JWT
    //        var securityKey = new SymmetricSecurityKey(GatekeeperOptions.JwtGeneration.SigningSecretBytes);
    //        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

    //        // authentication successful so generate jwt token
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = newIdentity,
    //            Expires = DateTime.Now.AddDays(1),
    //            SigningCredentials = signingCredentials,
    //            Issuer = GatekeeperOptions.JwtGeneration.Issuer,
    //            Audience = GatekeeperOptions.JwtGeneration.Audience
    //        };

    //        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
    //        string tokenString = tokenHandler.WriteToken(token);

    //        // Escribe JWT en un header
    //        HttpContext.Response.Headers.Add("Authorization", string.Format("Bearer {0}", tokenString));

    //        // Devuelve el recurso Session
    //        return Element<Session>(session);
    //    }
    //}
}
