using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public class RoleSeeder
    {
        public virtual async Task Seed(IResourceStore resourceStore, IRoleStore roleStore)
        {
            IEnumerable<string> resourceCodes = await resourceStore.GetAllCodes();

            bool isExistingAdminRole = roleStore.IsExisting(GtkRoleCodes.Admin).Result;
            if (!isExistingAdminRole)
            {
                Role defaultAdminRole = GetDefaultAdminRole(resourceCodes);
                await roleStore.Create(defaultAdminRole);
            }

            bool isExistingUserRole = roleStore.IsExisting(GtkRoleCodes.User).Result;
            if (!isExistingUserRole)
            {
                Role defaultUserRole = GetDefaultUserRole(resourceCodes);
                await roleStore.Create(defaultUserRole);
            }

            bool isExistingGuestRole = roleStore.IsExisting(GtkRoleCodes.Guest).Result;
            if (!isExistingGuestRole)
            {
                Role defaultGuestRole = GetDefaultGuestRole(resourceCodes);
                await roleStore.Create(defaultGuestRole);
            }
        }

        private Role GetDefaultUserRole(IEnumerable<string> resourceCodes)
        {
            var permissions = new List<Permission>();
            foreach (string resourceCode in resourceCodes)
                permissions.Add(new Permission(resourceCode, PermissionLevel.Read, PermissionLevel.None));

            Role role = new Role();
            role.Code = GtkRoleCodes.User;
            role.DisplayName = "User";
            role.Description = "Default user";
            role.Permissions = permissions;

            return role;
        }

        private Role GetDefaultAdminRole(IEnumerable<string> resourceCodes)
        {
            var permissions = new List<Permission>();
            foreach (string resourceCode in resourceCodes)
                permissions.Add(new Permission(resourceCode, PermissionLevel.Complete, PermissionLevel.Complete));

            Role role = new Role();
            role.Code = GtkRoleCodes.Admin;
            role.DisplayName = "Admin";
            role.Description = "Default admin";
            role.Permissions = permissions;

            return role;
        }

        private Role GetDefaultGuestRole(IEnumerable<string> resourceCodes)
        {
            var permissions = new List<Permission>();
            foreach (string resourceCode in resourceCodes)
            {
                // Por defecto el guest no puede hacer una verga
            }

            Role role = new Role();
            role.Code = GtkRoleCodes.Guest;
            role.DisplayName = "Guest";
            role.Description = "Default guest";
            role.Permissions = permissions;

            return role;
        }
    }
}
