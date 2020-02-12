using Microsoft.AspNetCore.Mvc;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Edit : RolesControllerFixture
    {
        public static IEnumerable<object[]> RoleUpdateFormList_Success()
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
        [MemberData(nameof(RoleUpdateFormList_Success))]
        public async Task Success(string roleCode, string displayName, string description, params Permission[] permissions)
        {
            List<Permission> permissionList = new List<Permission>();
            if (permissions != null && permissions.Count() > 0)
                permissionList = permissions.ToList();

            Role expected = _roleStoreBuilder.RoleList.FirstOrDefault(x => x.Code == roleCode);
            expected.DisplayName = displayName;
            expected.Description = description;
            expected.Permissions = permissionList;

            var form = new RoleUpdateForm(displayName, description, permissionList);
            ObjectResult editResult = await _defaultSut.Edit(roleCode, form) as ObjectResult;
            Role editRole = editResult.Value as Role;

            ObjectResult actualResult = await _defaultSut.GetByCode(roleCode) as ObjectResult;
            Role actual = actualResult.Value as Role;

            Assert.Equal(expected, actual, _roleComparer);
            Assert.Equal(expected, editRole, _roleComparer);
        }
    }
}
