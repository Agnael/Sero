using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Get_ByCode : RolesControllerFixture
    {
        // TODO: DESCOMENTÁ Y ARREGLÁ EL TEST, LO SAQUÉ PARA PROBAR ALGO DE HATEOAS
        //[Theory]
        //[InlineData("role_code_01")]
        //[InlineData("role_code_02")]
        //public async Task Success(string roleCode)
        //{
        //    // Arrange
        //    Role expected = _roleStoreBuilder
        //        .RoleList
        //        .FirstOrDefault(x => x.Code == roleCode);

        //    // Act
        //    Role actual = await _defaultSut.GetByCode(roleCode);

        //    // Assert
        //    Assert.Equal(expected, actual, _roleComparer);
        //}

        // TODO: DESCOMENTÁ Y ARREGLÁ EL TEST, LO SAQUÉ PARA PROBAR ALGO DE HATEOAS
        //[Fact]
        //public async Task UnexistingCode__ReturnsNullResource()
        //{
        //    // Act
        //    Role actual = await _defaultSut.GetByCode("randomUnexistingCode");
        //    Assert.Null(actual);
        //}

        [Fact]
        public async Task NullCode__Throws_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            {
                await _defaultSut.GetByCode(null);
            });
        }
    }
}
