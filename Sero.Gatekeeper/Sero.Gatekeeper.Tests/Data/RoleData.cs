using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public static class RoleData
    {
        public static Role Role_01_Admin => new Role("role_code_01", "Admin", "Administrator Role", new Permission[] {
            PermissionData.Resource_01_RW_R,
            PermissionData.Resource_02_RW_R,
            PermissionData.Resource_03_RW_R,
            PermissionData.Resource_04_RW_R,
            PermissionData.Resource_05_RW_R,
            PermissionData.Resource_06_RW_R,
            PermissionData.Resource_07_RW_R,
            PermissionData.Resource_08_RW_R,
            PermissionData.Resource_09_RW_R,
            PermissionData.Resource_10_RW_R,
        });

        public static Role Role_02_User => new Role("role_code_02", "User", "Normal user role", new Permission[] {
            PermissionData.Resource_01_R_R,
            PermissionData.Resource_03_R_R,
            PermissionData.Resource_05_R_R
        });
    }
}
