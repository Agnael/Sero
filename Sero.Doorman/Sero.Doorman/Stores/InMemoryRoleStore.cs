using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;
using Sero.Doorman.Controller;

namespace Sero.Doorman.Stores
{
    public class InMemoryRoleStore : IRoleStore
    {
        public readonly IList<Role> Roles;

        public InMemoryRoleStore(IList<Role> roles)
        {
            this.Roles = roles;
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

        public async Task<Page<Role>> Get(RolesFilter filter)
        {
            Func<Role, string> orderByPredicate = null;

            if (filter.SortBy == nameof(Role.Code))
                orderByPredicate = x => x.Code;
            else if (filter.SortBy.ToLower() == nameof(Role.Description).ToLower())
                orderByPredicate = x => x.Description;
            else if (filter.SortBy.ToLower() == nameof(Role.DisplayName).ToLower())
                orderByPredicate = x => x.DisplayName;

            IEnumerable<Role> query = Roles;

            if (filter.OrderBy.ToLower() == Order.DESC.ToLower())
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
                query = query.Where(x => x.Code.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.Description.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.DisplayName.ToLower().Contains(filter.TextSearch.ToLower()));

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

        public async Task<bool> IsUnique(string roleCode)
        {
            var result = !Roles.Any(x => x.Code == roleCode);
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
