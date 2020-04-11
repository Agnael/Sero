using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ISessionStore
    {
        Task<Session> Get(string credentialId);

        Task Create(Session session);
    }
}
