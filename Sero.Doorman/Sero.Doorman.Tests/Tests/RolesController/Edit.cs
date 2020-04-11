using Microsoft.AspNetCore.Mvc;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Sero.Core;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Edit : RolesControllerFixture
    {
        public static IEnumerable<object[]> Success_Data()
        {
            Permission[] validPermissions = new Permission[] { PermissionData.Resource_09_Read };
            yield return new object[] { RoleData.Role_01_Admin.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
            yield return new object[] { RoleData.Role_01_Admin.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
            yield return new object[] { RoleData.Role_01_Admin.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
            yield return new object[] { RoleData.Role_02_User.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
            yield return new object[] { RoleData.Role_02_User.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
            yield return new object[] { RoleData.Role_02_User.Code, ValUtil.GetDisplayName(), ValUtil.GetDescription(), validPermissions };
        }

        [Theory]
        [MemberData(nameof(Success_Data))]
        public async Task Success(string roleCode, string displayName, string description, params Permission[] permissions)
        {
            List<Permission> permissionList = new List<Permission>();
            if (permissions != null && permissions.Count() > 0)
                permissionList = permissions.ToList();

            Role expected = _roleStore.Roles.FirstOrDefault(x => x.Code == roleCode);
            expected.DisplayName = displayName;
            expected.Description = description;
            expected.Permissions = permissionList;

            var form = new RoleUpdateForm(displayName, description, permissionList);

            var actionResult = await _sut.Edit(roleCode, form);

            Assert.IsType<AcceptedAtActionResult>(actionResult);

            Role actual = actionResult.AsElementView<Role, Role>();

            Assert.Equal(expected, actual, _roleComparer);
        }
    }
}
