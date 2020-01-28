using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialService
    {
        Task<bool> IsValidCredential(string email, string password);
        Task<bool> IsValidCredential(Credential credential, string password);

        /// <summary>
        ///     Creates a Credential with only the default "DefaultRole" role.
        /// </summary>
        Task CreateCredential(string email, string password);

        /// <summary>
        ///     Creates a Credential with the default "DefaultRole" role and all the additional roles provided in the parameter.
        /// </summary>
        Task CreateCredential(string email, string password, IEnumerable<string> roleList);
    }
}