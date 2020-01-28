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
            Resource actual = await _defaultSut.GetByCode(resourceCode);

            // Assert
            Assert.Equal(expected, actual, _resourceComparer);
        }

        [Fact]
        public async Task UnexistingCode__ReturnsNullResource()
        {
            // Act
            Resource actual = await _defaultSut.GetByCode("randomUnexistingCode");
            Assert.Null(actual);
        }

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
