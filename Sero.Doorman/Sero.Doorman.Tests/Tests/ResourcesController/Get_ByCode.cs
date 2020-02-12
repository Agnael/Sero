using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Resources
{
    public class Get_ByCode : ResourcesControllerFixture
    {
        [Theory]
        [InlineData(ResourceData.RESOURCE_01_CODE)]
        [InlineData(ResourceData.RESOURCE_02_CODE)]
        [InlineData(ResourceData.RESOURCE_03_CODE)]
        [InlineData(ResourceData.RESOURCE_04_CODE)]
        [InlineData(ResourceData.RESOURCE_05_CODE)]
        public async Task Success(string resourceCode)
        {
            // Arrange
            Resource expected = _resourceStoreBuilder
                .ResourceList
                .FirstOrDefault(x => x.Code == resourceCode);

            // Act
            ObjectResult result = await _defaultSut.GetByCode(resourceCode) as ObjectResult;
            Resource actual = result.Value as Resource;

            // Assert
            Assert.Equal(expected, actual, _resourceComparer);
        }

        [Fact]
        public async Task UnexistingCode__NotFound()
        {
            // Act
            var result = await _defaultSut.GetByCode("randomUnexistingCode");
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NullCode__BadRequest()
        {
            var result = await _defaultSut.GetByCode(null);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
