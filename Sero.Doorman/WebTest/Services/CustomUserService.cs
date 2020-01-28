using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Services
{
    public class CustomUserService : ICustomUserService
    {
        public async Task<int> OnCredentialCreated(Credential createdCredential)
        {
            return createdCredential.Id;
        }
    }
}
