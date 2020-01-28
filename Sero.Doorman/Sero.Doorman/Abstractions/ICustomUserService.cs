using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICustomUserService
    {
        Task<int> OnCredentialCreated(Credential createdCredential);
    }
}
