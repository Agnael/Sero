using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;

namespace Sero.Sentinel.Storage
{
    public class InMemoryCredentialRoleStore : ICredentialRoleStore
    {
        public readonly IList<CredentialRole> Roles;

        public InMemoryCredentialRoleStore()
        {
            this.Roles = new List<CredentialRole>();
        }

        public InMemoryCredentialRoleStore(IList<CredentialRole> credentialRoles)
        {
            this.Roles = credentialRoles;
        }

        public async Task Create(CredentialRole credentialRole)
        {
            Roles.Add(credentialRole);
        }

        public async Task Delete(string credentialRoleCode)
        {
            var found = Roles.FirstOrDefault(x => x.Code == credentialRoleCode);
            
            if(found != null)
            {
                Roles.Remove(found);
            }
        }

        public async Task<IPage<CredentialRole>> Get(CredentialRoleFilter filter)
        {
            Func<CredentialRole, object> orderByPredicate = null;

            if (filter.SortBy == CredentialRoleSorting.Code)
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == CredentialRoleSorting.Description)
                orderByPredicate = x => x.Description;

            IEnumerable<CredentialRole> query = Roles;

            if (filter.OrderBy == Order.Desc)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.FreeText))
            {
                string searched = filter.FreeText.ToLower();

                query = 
                    query
                    .Where(x => 
                        x.Code.ToLower().Contains(searched)
                        || x.Description.ToLower().Contains(searched));
            }

            if (!string.IsNullOrEmpty(filter.Code))
            {
                string searched = filter.Code.ToLower();
                query = query.Where(x => x.Code.ToLower().Contains(searched));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                string searched = filter.Description.ToLower();
                query = query.Where(x => x.Description.ToLower().Contains(searched));
            }

            var count = query.Count();

            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<CredentialRole>(count, list);
        }

        public async Task<CredentialRole> Get(string credentialRoleCode)
        {
            var found = Roles.FirstOrDefault(x => x.Code == credentialRoleCode);
            return found;
        }

        public async Task<bool> IsExisting(string credentialRoleCode)
        {
            bool isExisting = Roles.Any(x => x.Code == credentialRoleCode);
            return isExisting;
        }

        public async Task Update(CredentialRole credentialRole)
        {
            var found = Roles.FirstOrDefault(x => x.Code == credentialRole.Code);

            if(found != null)
            {
                found.Description = credentialRole.Description;
            }
        }
    }
}
