using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialStore
    {
        Task<IEnumerable<Credential>> FetchAsync(CredentialsFilter filter);
        Task<int> CountAsync(CredentialsFilter filter);

        Task<Credential> FetchAsync(string email);
        Task<Credential> FetchAsync(Guid credentialId);

        Task<bool> IsExistingAsync(string email);
        Task<bool> IsExistingAsync(Guid credentialId);

        Task SaveAsync(Credential user);
        Task UpdateAsync(Credential user);
    }
}
