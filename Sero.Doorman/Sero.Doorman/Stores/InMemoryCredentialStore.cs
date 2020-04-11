using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;
using Sero.Doorman.Controller;

namespace Sero.Doorman
{
    public class InMemoryCredentialStore : ICredentialStore
    {
        public readonly IList<Credential> Credentials;

        public InMemoryCredentialStore(IList<Credential> credentials)
        {
            this.Credentials = new List<Credential>(credentials);
        }

        public async Task<Credential> GetByEmail(string email)
        {
            var result = Credentials.FirstOrDefault(x => x.Email == email);
            return result;
        }

        public async Task<Credential> Get(string CredentialId)
        {
            var result = Credentials.FirstOrDefault(x => x.CredentialId == CredentialId.ToLower());
            return result;
        }
        
        public async Task<Page<Credential>> Get(CredentialFilter filter)
        {
            Func<Credential, object> orderByPredicate = null;

            if (filter.SortBy == CredentialSorting.CredentialId)
                orderByPredicate = x => x.CredentialId;
            else if (filter.SortBy == CredentialSorting.BirthDate)
                orderByPredicate = x => x.BirthDate;
            else if (filter.SortBy == CredentialSorting.CreationDate)
                orderByPredicate = x => x.CreationDate;
            else if (filter.SortBy == CredentialSorting.Email)
                orderByPredicate = x => x.Email;

            IEnumerable<Credential> query = Credentials;

            if (filter.OrderBy == Order.Desc)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.FreeText))
            {
                string searched = filter.FreeText.ToLower();
                query = query.Where(x => x.Email.ToLower().Contains(searched)
                                        || x.CredentialId.Contains(searched)
                                        || x.DisplayName.Contains(searched)
                                        || x.Roles.Any(y => y.Code.ToLower().Contains(searched)
                                                            || y.DisplayName.ToLower().Contains(searched)));
            }

            if(!string.IsNullOrEmpty(filter.Email))
            {
                string searched = filter.FreeText.ToLower();
                query = query.Where(x => x.Email.ToLower().Contains(searched));
            }

            if (filter.CreationDateMin.HasValue)
                query = query.Where(x => x.CreationDate >= filter.CreationDateMin);

            if (filter.CreationDateMax.HasValue)
                query = query.Where(x => x.CreationDate <= filter.CreationDateMax);

            if (filter.BirthDateMin.HasValue)
                query = query.Where(x => x.BirthDate >= filter.BirthDateMin);

            if (filter.BirthDateMax.HasValue)
                query = query.Where(x => x.BirthDate <= filter.BirthDateMax);

            if(filter.RoleCodes.Count() > 0)
                query = query.Where(x => filter.RoleCodes.All(y => x.Roles.Any(z => z.Code == y)));

            var count = query.Count();
            
            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<Credential>(count, list);
        }

        public async Task<bool> IsExistingByEmail(string email)
        {
            var result = Credentials.Any(x => x.Email == email);
            return result;
        }

        public async Task<bool> IsExistingByCredentialId(string CredentialId)
        {
            var result = Credentials.Any(x => x.CredentialId == CredentialId);
            return result;
        }

        public async Task Create(Credential user)
        {
            Credentials.Add(user);
        }

        public async Task Update(Credential user)
        {
            var found = Credentials.FirstOrDefault(x => x.CredentialId.Equals(user.CredentialId));
            found.DisplayName = user.DisplayName;
            found.Roles = user.Roles;
            found.PasswordSalt = user.PasswordSalt;
            found.PasswordHash = user.PasswordHash;
            found.Email = user.Email;
            found.BirthDate = user.BirthDate;
        }

        public async Task<Page<Role>> GetRoles(string CredentialId, RoleFilter filter)
        {
            var credential = await this.Get(CredentialId);

            if (credential == null)
                return new Page<Role>();

            var roleStore = new InMemoryRoleStore(credential.Roles);

            var roles = await roleStore.Get(filter);
            return roles;

        }

        public async Task<bool> IsExistingByRoleCode(string roleCode)
        {
            var result = Credentials.Any(x => x.Roles.Any(y => y.Code == roleCode));
            return result;
        }
    }
}
