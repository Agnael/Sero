using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public class SessionService
    {
        public readonly ICredentialStore CredentialStore;
        public readonly ILoginAttemptStore LoginAttemptStore;
        public readonly IRequestInfo RequestInfo;
        public readonly ICredentialPenaltyStore CredentialPenaltyStore;
        public readonly ISessionStore SessionStore;

        public SessionService(
            ICredentialStore credentialStore,
            ILoginAttemptStore loginAttemptStore,
            IRequestInfo reqInfo,
            ICredentialPenaltyStore credentialPenaltyStore,
            ISessionStore sessionStore)
        {
            this.CredentialStore = credentialStore;
            this.LoginAttemptStore = loginAttemptStore;
            this.RequestInfo = reqInfo;
            this.CredentialPenaltyStore = credentialPenaltyStore;
            this.SessionStore = sessionStore;
        }
            
        public async Task<Session> SignIn(Credential credential, string password, bool allowSelfRenewal)
        {
            //string newCalculatedHash = HashingUtil.GenerateHash(password, credential.PasswordSalt);

            //if (newCalculatedHash != credential.PasswordHash)
            //{
            //    LoginAttempt attempt = new LoginAttempt(this.RequestInfo.RemoteIpAddress, DateTime.UtcNow);
            //    await LoginAttemptStore.Create(attempt);

            //    return null;
            //}


            Session newSession = new Session();
            //newSession.CredentialId = credential.CredentialId;
            //newSession.LoginDate = DateTime.UtcNow;
            //newSession.ExpirationDate = DateTime.UtcNow.AddDays(1);
            //newSession.LastActiveDate = newSession.LoginDate;
            //newSession.AllowSelfRenewal = allowSelfRenewal;

            //await SessionStore.Create(newSession);

            return newSession;
        }
    }
}
