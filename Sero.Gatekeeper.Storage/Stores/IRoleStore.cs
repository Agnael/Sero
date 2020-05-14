using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public interface IRoleStore
    {
        Task<IPage<Role>> Get(RoleFilter filter);
        Task<Role> Get(string roleCode);

        Task Update(Role role);
        Task Create(Role role);
        Task Delete(string roleCode);
        Task<bool> IsExisting(string roleCode);
    }
}
