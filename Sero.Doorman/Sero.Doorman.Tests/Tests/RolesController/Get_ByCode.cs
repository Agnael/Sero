using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Sero.Core;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Get_ByCode : RolesControllerFixture
    {
        [Theory]
        [InlineData("role_code_01")]
        [InlineData("role_code_02")]
        public async Task Success(string roleCode)
        {
            // Arrange
            Role expected = _roleStore
                .Roles
                .FirstOrDefault(x => x.Code == roleCode);

            // Act
            var actionResult = await _sut.GetByCode(roleCode);
            Role actual = actionResult.AsElementView<Role, Role>();

            // Assert
            Assert.Equal(expected, actual, _roleComparer);
        }

        [Fact]
        public async Task UnexistingCode__404NotFound()
        {
            // Act
            IActionResult result = await _sut.GetByCode("randomUnexistingCode");
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NullCode__Throws_ArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            {
                await _sut.GetByCode(null);
            });
        }
    }
}
