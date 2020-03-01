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

        public async Task<Credential> FetchAsync(string email)
        {
            var result = Credentials.FirstOrDefault(x => x.Email == email);
            return result;
        }

        public async Task<Credential> FetchAsync(Guid credentialId)
        {
            var result = Credentials.FirstOrDefault(x => x.CredentialId == credentialId);
            return result;
        }

        public async Task<int> CountAsync(CredentialsFilter filter)
        {
            IEnumerable<Credential> query = Credentials;
            Func<Credential, object> orderByPredicate = null;

            if (filter.SortBy.ToLower() == nameof(Credential.CredentialId))
                orderByPredicate = x => x.CredentialId;
            else if (filter.SortBy.ToLower() == nameof(Credential.BirthDate).ToLower())
                orderByPredicate = x => x.BirthDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.CreationDate).ToLower())
                orderByPredicate = x => x.CreationDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.Email).ToLower())
                orderByPredicate = x => x.Email;

            if (filter.OrderBy.ToLower() == Order.DESC.ToLower())
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
            {
                string searched = filter.TextSearch.ToLower();
                query = query.Where(x => x.Email.ToLower().Contains(searched)
                                        || x.Roles.Any(y => y.Code.ToLower().Contains(searched)
                                                            || y.DisplayName.ToLower().Contains(searched)));
            }

            if (!string.IsNullOrEmpty(filter.Email))
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

            var result = query.Count();
            return result;
        }

        public async Task<IEnumerable<Credential>> FetchAsync(CredentialsFilter filter)
        {
            IEnumerable<Credential> query = Credentials;
            Func<Credential, object> orderByPredicate = null;

            if (filter.SortBy.ToLower() == nameof(Credential.CredentialId))
                orderByPredicate = x => x.CredentialId;
            else if (filter.SortBy.ToLower() == nameof(Credential.BirthDate).ToLower())
                orderByPredicate = x => x.BirthDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.CreationDate).ToLower())
                orderByPredicate = x => x.CreationDate;
            else if (filter.SortBy.ToLower() == nameof(Credential.Email).ToLower())
                orderByPredicate = x => x.Email;

            if (filter.OrderBy.ToLower() == Order.DESC.ToLower())
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
            {
                string searched = filter.TextSearch.ToLower();
                query = query.Where(x => x.Email.ToLower().Contains(searched)
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

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return result;
        }

        public async Task<bool> IsExistingAsync(string email)
        {
            var result = Credentials.Any(x => x.Email == email);
            return result;
        }

        public async Task<bool> IsExistingAsync(Guid credentialId)
        {
            var result = Credentials.Any(x => x.CredentialId == credentialId);
            return result;
        }

        public async Task SaveAsync(Credential user)
        {
            Credentials.Add(user);
        }

        public async Task UpdateAsync(Credential user)
        {
            var found = Credentials.FirstOrDefault(x => x.CredentialId.Equals(user.CredentialId));
            found.Roles = user.Roles;
            found.PasswordSalt = user.PasswordSalt;
            found.PasswordHash = user.PasswordHash;
            found.Email = user.Email;
            found.BirthDate = user.BirthDate;
        }
    }
}
