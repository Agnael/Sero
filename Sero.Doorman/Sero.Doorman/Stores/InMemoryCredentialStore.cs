using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sero.Core;
using Sero.Doorman.Controller;

namespace Sero.Doorman.Stores
{
    public class InMemoryCredentialStore : ICredentialStore
    {
        public readonly IList<Credential> Credentials;

        public InMemoryCredentialStore(IList<Credential> credentials)
        {
            this.Credentials = credentials;
        }

        public async Task<Credential> GetByEmail(string email)
        {
            var result = Credentials.FirstOrDefault(x => x.Email == email);
            return result;
        }

        public async Task<Credential> Get(string username)
        {
            var result = Credentials.FirstOrDefault(x => x.Username == username.ToLower());
            return result;
        }
        
        public async Task<Page<Credential>> Get(CredentialsFilter filter)
        {
            Func<Credential, object> orderByPredicate = null;

            if (filter.SortBy.ToLower() == nameof(Credential.Username))
                orderByPredicate = x => x.Username;
            else if (filter.SortBy.ToLower() == nameof(Credential.BirthDate).ToLower())
                orderByPredicate = x => x.BirthDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.CreationDate).ToLower())
                orderByPredicate = x => x.CreationDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.Email).ToLower())
                orderByPredicate = x => x.Email;

            IEnumerable<Credential> query = Credentials;

            if (filter.OrderBy.ToLower() == Order.DESC.ToLower())
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
            {
                string searched = filter.TextSearch.ToLower();
                query = query.Where(x => x.Email.ToLower().Contains(searched)
                                        || x.Username.Contains(searched)
                                        || x.DisplayName.Contains(searched)
                                        || x.Roles.Any(y => y.Code.ToLower().Contains(searched)
                                                            || y.DisplayName.ToLower().Contains(searched)));
            }

            if(!string.IsNullOrEmpty(filter.Email))
            {
                string searched = filter.TextSearch.ToLower();
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

            var count = query.Count();
            
            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<Credential>(count, list);
        }

        public async Task<bool> IsUniqueEmail(string email)
        {
            var result = !Credentials.Any(x => x.Email == email);
            return result;
        }

        public async Task<bool> IsUniqueUsername(string username)
        {
            var result = !Credentials.Any(x => x.Username == username);
            return result;
        }

        public async Task Save(Credential user)
        {
            Credentials.Add(user);
        }

        public async Task Update(Credential user)
        {
            var found = Credentials.FirstOrDefault(x => x.Username.Equals(user.Username));
            found.DisplayName = user.DisplayName;
            found.Roles = user.Roles;
            found.PasswordSalt = user.PasswordSalt;
            found.PasswordHash = user.PasswordHash;
            found.Email = user.Email;
            found.BirthDate = user.BirthDate;
        }

        public async Task<Page<Role>> GetRoles(string username, RolesFilter filter)
        {
            var credential = await this.Get(username);

            if (credential == null)
                return new Page<Role>();

            var roleStore = new InMemoryRoleStore(credential.Roles);

            var roles = await roleStore.Get(filter);
            return roles;

        }
    }
}
