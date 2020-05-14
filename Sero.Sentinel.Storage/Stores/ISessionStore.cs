using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public interface ISessionStore
    {
        Task<Page<Session>> Get(SessionFilter sessionFilter);
        Task<Session> Get(string credentialId, string deviceClass, string deviceName, string agentName, string agentVersion);

        Task Create(Session session);
        Task Update(Session session);
    }
}
