using Microsoft.AspNetCore.Mvc.RazorPages;
using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public interface ICredentialRoleStore
    {
        Task<IPage<CredentialRole>>Get(CredentialRoleFilter filter);
        Task<CredentialRole> Get(string credentialRoleCode);
        Task<bool> IsExisting(string credentialRoleCode);

        Task Create(CredentialRole credentialRole);
        Task Update(CredentialRole credentialRole);
        Task Delete(string credentialRoleCode);
    }
}
