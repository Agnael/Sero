using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public interface IUserStore
    {
        Task<User> Get(string okAuthCredentialId);

        Task<IPage<Role>> GetRoles(string okAuthCredentialId, RoleFilter filter);
        Task<IList<Role>> GetRoles(string okAuthCredentialId);
    }
}
