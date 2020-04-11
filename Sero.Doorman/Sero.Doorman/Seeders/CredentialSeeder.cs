using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public class CredentialSeeder
    {
        public virtual async Task Seed(IRoleStore roleStore, ICredentialStore credentialStore)
        {

            bool isExistingAnyAdminCredentials = credentialStore.IsExistingByRoleCode(Constants.RoleCodes.Admin).Result;
            if (!isExistingAnyAdminCredentials)
            {
                Role defaultAdminRole = roleStore.Get(Constants.RoleCodes.Admin).Result;

                string adminCredentialId = Constants.CredentialIds.Admin;

                Credential admin = new Credential();
                admin.BirthDate = new DateTime(2000, 1, 1);
                admin.CreationDate = DateTime.UtcNow;
                admin.DisplayName = adminCredentialId;
                admin.CredentialId = adminCredentialId;
                admin.Email = adminCredentialId+"@admin.com";
                admin.PasswordSalt = HashingUtil.GenerateSalt();
                admin.PasswordHash = HashingUtil.GenerateHash(adminCredentialId, admin.PasswordSalt);
                admin.Roles = new List<Role> { defaultAdminRole };

                await credentialStore.Create(admin);
            }
        }
    }
}
