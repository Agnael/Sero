using Sero.Core;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public class CredentialSeeder
    {
        public virtual async Task Seed(
            ICredentialRoleStore credentialRoleStore, 
            ICredentialStore credentialStore)
        {
            await SeedGlobalAdministrator(credentialRoleStore, credentialStore);
        }

        private async Task SeedGlobalAdministrator(
            ICredentialRoleStore credentialRoleStore,
            ICredentialStore credentialStore)
        {
            bool isExistingAnyAdminCredentials =
                await credentialStore.IsExistingByCredentialRoleCode(SentinelCredentialRoleCodes.GlobalAdmin);

            if (!isExistingAnyAdminCredentials)
            {
                string adminCredentialId = SentinelCredentialIds.Admin;

                CredentialRole adminRole =
                    await credentialRoleStore.Get(SentinelCredentialRoleCodes.GlobalAdmin);

                Credential admin = new Credential();
                admin.BirthDate = new DateTime(2000, 1, 1);
                admin.CreationDate = DateTime.UtcNow;
                admin.DisplayName = adminCredentialId;
                admin.CredentialId = adminCredentialId;
                admin.Email = adminCredentialId + "@admin.com";
                admin.PasswordSalt = HashingUtil.GenerateSalt();
                admin.PasswordHash = HashingUtil.GenerateHash(adminCredentialId, admin.PasswordSalt);
                admin.Roles = new List<CredentialRole> { adminRole };

                await credentialStore.Create(admin);
            }
        }
    }
}
