using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests
{
    public static class RoleData
    {
        public static Role Role_01_Admin => new Role("role_code_01", "Admin", "Administrator Role", new Permission[] {
            PermissionData.Resource_01_ReadWrite,
            PermissionData.Resource_02_ReadWrite,
            PermissionData.Resource_03_ReadWrite,
            PermissionData.Resource_04_ReadWrite,
            PermissionData.Resource_05_ReadWrite,
            PermissionData.Resource_06_ReadWrite,
            PermissionData.Resource_07_ReadWrite,
            PermissionData.Resource_08_ReadWrite,
            PermissionData.Resource_09_ReadWrite,
            PermissionData.Resource_10_ReadWrite,
        });

        public static Role Role_02_User => new Role("role_code_02", "User", "Normal user role", new Permission[] {
            PermissionData.Resource_01_Read,
            PermissionData.Resource_03_Read,
            PermissionData.Resource_05_Read
        });
    }
}
