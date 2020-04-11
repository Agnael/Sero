using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;
using Sero.Doorman.Controller;

namespace Sero.Doorman
{
    public class InMemorySessionStore : ISessionStore
    {
        public readonly IList<Session> Sessions;

        public InMemorySessionStore()
        {
            this.Sessions = new List<Session>();
        }

        public InMemorySessionStore(IList<Session> sessions)
        {
            this.Sessions = sessions;
        }

        public async Task Create(Session session)
        {
            this.Sessions.Add(session);
        }

        public async Task<Session> Get(string credentialId)
        {
            var result = 
                Sessions
                .OrderByDescending(x => x.LoginDate)
                .FirstOrDefault(x => 
                    x.CredentialId == credentialId
                    && x.ExpirationDate < DateTime.UtcNow);

            return result;
        }
    }
}
