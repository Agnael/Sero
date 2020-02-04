using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sero.Doorman.Controller;
using Sero.Doorman.Tests.Controllers.Roles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Create : RolesControllerFixture
    {
        public static IEnumerable<object[]> RoleFormTestList_Success => new[]
        {
            new object[] { "newRole_01", "New role 01", "New role 01 desc[]", new Permission[] {
                    PermissionData.Resource_01_ReadWrite,
                    PermissionData.Resource_02_Read,
                    PermissionData.Resource_04_ReadWrite,
                    PermissionData.Resource_09_Read
            }},
            new object[] { "newRole_02", "New role 02", "New role 02 desc[]", new Permission[] {
                    PermissionData.Resource_01_Read,
                    PermissionData.Resource_02_ReadWrite,
            }}
        };

        // TODO: DESCOMENTÁ Y ARREGLÁ EL TEST, LO SAQUÉ PARA PROBAR ALGO DE HATEOAS
        //// TENER EN CUENTA SOBRE MemberData:
        //// https://stackoverflow.com/questions/30574322/memberdata-tests-show-up-as-one-test-instead-of-many
        //// Este test va a verse como 1 solo en el TestExplorer, pero van a correrse todas las variantes deseadas
        //[Theory]
        //[MemberData(nameof(RoleFormTestList_Success))]
        //public async Task Success(string code, string name, string description, params Permission[] permissions)
        //{
        //    // Arrange
        //    Role expected = new Role(code, name, description, permissions);

        //    // Act
        //    IActionResult result = await _defaultSut.Create(new RoleCreateForm(code, name, description, permissions));
        //    Role actual = await _defaultSut.GetByCode(code);

        //    // Assert
        //    Assert.IsType<StatusCodeResult>(result);
        //    Assert.Equal((int)HttpStatusCode.Created, ((StatusCodeResult)result).StatusCode);
        //    Assert.Equal(expected, actual, _roleComparer);
        //}

        public static IEnumerable<object[]> RoleFormTestList_InvalidForm()
        {
            string validCode = ValUtil.GetCode();
            string validName = ValUtil.GetDisplayName();
            Permission[] validPermissions = new Permission[] { PermissionData.Resource_09_Read };

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
            
            IActionResult actual = await _defaultSut.Create(form);

            Assert.IsType<ObjectResult>(actual);
            Assert.Equal(StatusCodes.Status400BadRequest, (actual as ObjectResult).StatusCode);
        }
    }
}
