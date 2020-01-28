using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialStore
    {
        Task<Credential> FetchAsync(string email);
        Task<Credential> FetchAsync(int idUser);

        Task<bool> ExistsEmailAsync(string email);

        Task<int> SaveAsync(Credential user);
    }
}
