using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialStore
    {
        Task<Page<Credential>> Get(CredentialsFilter filter);
        Task<Credential> Get(string username);
        Task<Credential> GetByEmail(string email);

        Task<Page<Role>> GetRoles(string username, RolesFilter filter);

        Task<bool> IsUniqueEmail(string email);
        Task<bool> IsUniqueUsername(string username);

        Task Save(Credential user);
        Task Update(Credential user);
    }
}
