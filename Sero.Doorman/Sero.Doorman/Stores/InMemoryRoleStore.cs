using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task CreateAsync(Role role)
        {
            Roles.Add(role);
        }

        public async Task DeleteAsync(string roleCode)
        {
            var foundRole = Roles.FirstOrDefault(x => x.Code == roleCode);
            Roles.Remove(foundRole);
        }

        public async Task<int> CountAsync(RolesFilter filter)
        {
            IEnumerable<Role> query = Roles;
            Func<Role, string> orderByPredicate = null;

            // Construye el predicate de ordenamiento en función del nombre de campo, es horrendo para este caso puntual pero
            // haciendolo así se tiene la flexibilidad de poder usar otro tipo diferente al modelo Resource.
            if (filter.SortBy == nameof(Role.Code))
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == nameof(Role.Description))
                orderByPredicate = x => x.Description;
            else if (filter.SortBy == nameof(Role.DisplayName))
                orderByPredicate = x => x.DisplayName;

            if (filter.OrderBy == Order.DESC)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
                query = query.Where(x => x.Code.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.Description.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.DisplayName.ToLower().Contains(filter.TextSearch.ToLower()));

            var result = query.Count();

            return result;
        }

        public async Task<ICollection<Role>> FetchAsync(RolesFilter filter)
        {
            IEnumerable<Role> query = Roles;
            Func<Role, string> orderByPredicate = null;

            // Construye el predicate de ordenamiento en función del nombre de campo, es horrendo para este caso puntual pero
            // haciendolo así se tiene la flexibilidad de poder usar otro tipo diferente al modelo Resource.
            if (filter.SortBy == nameof(Role.Code))
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == nameof(Role.Description))
                orderByPredicate = x => x.Description;
            else if (filter.SortBy == nameof(Role.DisplayName))
                orderByPredicate = x => x.DisplayName;

            if (filter.OrderBy == Order.DESC)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
                query = query.Where(x => x.Code.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.Description.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.DisplayName.ToLower().Contains(filter.TextSearch.ToLower()));

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return result;
        }

        public async Task<Role> FetchAsync(string roleCode)
        {
            var result = Roles.FirstOrDefault(x => x.Code == roleCode);
            return result;
        }

        public async Task<bool> IsExistingAsync(string roleCode)
        {
            var result = Roles.Any(x => x.Code == roleCode);
            return result;
        }

        public async Task UpdateAsync(Role role)
        {
            var existingRole = Roles.FirstOrDefault(x => x.Code == role.Code);
            existingRole.Description = role.Description;
            existingRole.DisplayName = role.DisplayName;
            existingRole.Permissions = role.Permissions;
        }
    }
}
