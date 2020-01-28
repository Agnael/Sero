using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface IRoleStore
    {
        Task<ICollection<Role>> FetchAsync(RolesFilter filter);
        Task<Role> FetchAsync(string roleCode);
        Task UpdateAsync(Role role);
        Task CreateAsync(Role role);
        Task DeleteAsync(string roleCode);
        Task<bool> IsExistingAsync(string roleCode);
    }
}
