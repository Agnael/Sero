using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Gatekeeper.Controller;
using Sero.Gatekeeper.Tests.Controllers.Roles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Sero.Gatekeeper.Tests.Controllers.Roles
{
    public class Create : RolesControllerFixture
    {
        public static IEnumerable<object[]> RoleFormTestList_Success => new[]
        {
            new object[] { "newRole_01", "New role 01", "New role 01 desc[]", new Permission[] {
                    PermissionData.Resource_01_RW_R,
                    PermissionData.Resource_02_R_R,
                    PermissionData.Resource_04_RW_R,
                    PermissionData.Resource_09_R_R
            }},
            new object[] { "newRole_02", "New role 02", "New role 02 desc[]", new Permission[] {
                    PermissionData.Resource_01_R_R,
                    PermissionData.Resource_02_RW_R,
            }}
        };

        // TENER EN CUENTA SOBRE MemberData:
        // https://stackoverflow.com/questions/30574322/memberdata-tests-show-up-as-one-test-instead-of-many
        // Este test va a verse como 1 solo en el TestExplorer, pero van a correrse todas las variantes deseadas
        [Theory]
        [MemberData(nameof(RoleFormTestList_Success))]
        public async Task Success(string code, string name, string description, params Permission[] permissions)
        {
            Role expected = new Role(code, name, description, permissions);
            var form = new RoleCreateForm(code, name, description, permissions);

            var actionResult = await _sut.Create(form);
            
            Assert.IsType<CreatedAtActionResult>(actionResult);
            Role actual = actionResult.AsElementView<Role, Role>();
            Assert.Equal(expected, actual, _roleComparer);
        }

        public static IEnumerable<object[]> RoleFormTestList_InvalidForm()
        {
            string validCode = ValUtil.GetCode();
            string validName = ValUtil.GetDisplayName();
            Permission[] validPermissions = new Permission[] { PermissionData.Resource_09_R_R };

            yield return new object[] { ValUtil.GetCode("!<[&%>"), validName, null, validPermissions };
            yield return new object[] { ValUtil.GetCode(Constants.Validation.Code_Length_Max+1), validName, null, validPermissions};
            yield return new object[] { validCode, ValUtil.GetDisplayName("<[(&}%"), null, validPermissions};
            yield return new object[] { validCode, validName, ValUtil.GetDescription("!%&$/(#$\\-[¨*´<"), validPermissions};
            yield return new object[] { validCode, validName, ValUtil.GetDescription(Constants.Validation.Description_Length_Max+1), validPermissions};
            yield return new object[] { validCode, validName, null, new Permission[] {}};
            yield return new object[] { validCode, validName, null, null };
            yield return new object[] { validCode, ValUtil.GetDisplayName(Constants.Validation.DisplayName_Length_Max+1), null, validPermissions};
        }

        [Theory]
        [MemberData(nameof(RoleFormTestList_InvalidForm))]
        public async Task Invalid_CreateForm__ReturnsValidationErrorView(string code, string name, string description, params Permission[] permissions)
        {
            var form = new RoleCreateForm(code, name, description, permissions);
            
            ObjectResult result = await _sut.Create(form) as ObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result.Value);
        }
    }
}
