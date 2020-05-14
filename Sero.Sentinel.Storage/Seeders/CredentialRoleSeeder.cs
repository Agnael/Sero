using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage.Seeders
{
    public class CredentialRoleSeeder
    {
        public virtual async Task Seed(ICredentialRoleStore store)
        {
            await SeedGlobalAdministratorRole(store);
            await SeetRegularUserRole(store);
        }

        private async Task SeedGlobalAdministratorRole(ICredentialRoleStore store)
        {
            bool isGlobalAdminRoleExisting = await store.IsExisting(SentinelCredentialRoleCodes.GlobalAdmin);

            if (!isGlobalAdminRoleExisting)
            {
                CredentialRole role = new CredentialRole();
                role.Code = SentinelCredentialRoleCodes.GlobalAdmin;
                role.Description = "Godlike administrator, completely unrestricted";

                await store.Create(role);
            }
        }

        private async Task SeetRegularUserRole(ICredentialRoleStore store)
        {
            bool isRegularUserRoleExisting = await store.IsExisting(SentinelCredentialRoleCodes.RegularUser);

            if (!isRegularUserRoleExisting)
            {
                CredentialRole role = new CredentialRole();
                role.Code = SentinelCredentialRoleCodes.RegularUser;
                role.Description = "Regular authenticated user";

                await store.Create(role);
            }
        }
    }
}
