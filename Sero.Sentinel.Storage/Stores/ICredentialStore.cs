using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public interface ICredentialStore
    {
        Task<IPage<Credential>> Get(CredentialFilter filter);
        Task<Credential> Get(string credentialId);
        Task<Credential> GetByEmail(string email);

        Task<bool> IsExistingByEmail(string email);
        Task<bool> IsExistingByCredentialId(string credentialId);
        Task<bool> IsExistingByCredentialRoleCode(string credentialRoleCode);

        Task Create(Credential user);
        Task Update(Credential user);

        Task<IPage<CredentialRole>> GetRoles(string credentialId, CredentialRoleFilter credentialRoleFilter);
        Task<IPage<Session>> GetSessions(string credentialId, SessionFilter sessionFilter);
    }
}
