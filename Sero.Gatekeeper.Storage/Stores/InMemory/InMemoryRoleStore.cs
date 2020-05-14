using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public class InMemoryRoleStore : IRoleStore
    {
        public readonly IList<Role> Roles;

        public InMemoryRoleStore(IList<Role> roles)
        {
            this.Roles = new List<Role>(roles);
        }

        public async Task Create(Role role)
        {
            Roles.Add(role);
        }

        public async Task Delete(string roleCode)
        {
            var foundRole = Roles.FirstOrDefault(x => x.Code == roleCode);
            Roles.Remove(foundRole);
        }

        public async Task<IPage<Role>> Get(RoleFilter filter)
        {
            Func<Role, string> orderByPredicate = null;

            if (filter.SortBy == RoleSorting.Code)
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == RoleSorting.Description)
                orderByPredicate = x => x.Description;
            else if (filter.SortBy == RoleSorting.DisplayName)
                orderByPredicate = x => x.DisplayName;

            IEnumerable<Role> query = Roles;

            if (filter.OrderBy == Order.Desc)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.FreeText))
                query = query.Where(x => x.Code.ToLower().Contains(filter.FreeText.ToLower())
                                        || x.Description.ToLower().Contains(filter.FreeText.ToLower())
                                        || x.DisplayName.ToLower().Contains(filter.FreeText.ToLower()));

            int count = query.Count();

            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<Role>(count, list);
        }

        public async Task<Role> Get(string roleCode)
        {
            var result = Roles.FirstOrDefault(x => x.Code == roleCode);
            return result;
        }

        public async Task<bool> IsExisting(string roleCode)
        {
            var result = Roles.Any(x => x.Code == roleCode);
            return result;
        }

        public async Task Update(Role role)
        {
            var existingRole = Roles.FirstOrDefault(x => x.Code == role.Code);
            existingRole.Description = role.Description;
            existingRole.DisplayName = role.DisplayName;
            existingRole.Permissions = role.Permissions;
        }
    }
}
