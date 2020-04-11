using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialStore
    {
        Task<Page<Credential>> Get(CredentialFilter filter);
        Task<Credential> Get(string CredentialId);
        Task<Credential> GetByEmail(string email);

        Task<Page<Role>> GetRoles(string CredentialId, RoleFilter filter);

        Task<bool> IsExistingByEmail(string email);
        Task<bool> IsExistingByCredentialId(string CredentialId);
        Task<bool> IsExistingByRoleCode(string roleCode);

        Task Create(Credential user);
        Task Update(Credential user);
    }
}
