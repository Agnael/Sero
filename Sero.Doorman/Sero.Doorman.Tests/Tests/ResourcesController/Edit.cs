using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Doorman.Controller;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Resources
{
    public class Edit : ResourcesControllerFixture
    {
        [Theory]
        [InlineData(ResourceData.RESOURCE_01_CODE, "newkat1", null)]
        [InlineData(ResourceData.RESOURCE_04_CODE, null, "newDesk2")]
        [InlineData(ResourceData.RESOURCE_05_CODE, "newkat3", "newdesk3")]
        public async Task Success(string resourceCode, string newCategory, string newDescription)
        {
            // Arrange
            Resource expected = _resourceStore.Resources.FirstOrDefault(x => x.Code == resourceCode);
            expected.Category = newCategory;
            expected.Description = newDescription;

            // Act
            ResourceUpdateForm form = new ResourceUpdateForm(newCategory, newDescription);

            var result = _sut.Edit(resourceCode, form).Result.AsAcceptedAtActionResult();

            //ObjectResult updatedResult = await _defaultSut.GetByCode(resourceCode) as ObjectResult;
            //Resource actual = updatedResult.Value as Resource;

            Resource actual = result.GetElement<Resource>();

            // Assert
            Assert.Equal((int)HttpStatusCode.Accepted, result.StatusCode);
            Assert.Equal(expected, actual, _resourceComparer);
        }

        [Theory]
        [InlineData("{invalidCategory}", null)]
        [InlineData("-invalidCategory-", "newDesk2@asd.com")]
        [InlineData("%invalidCategory%", "[validDesc}}{()()//@%]")]
        [InlineData(null, "[validDesc}}{()()//@%]")]
        [InlineData("newCategory", "TooLongDescRiption_zpeptbzLfY1I804ks1xGEiVQRdFAafnlfjHdsCR1r6oNgsJwLeEzRvoPFXxFo2hes8suN758iC75CdrqinSK5nTYG7nMg6rD9b0rO3zMjjHRuLtKxu0t81KysyxikMJTKW7aCYT63Szwy34I0dDGmQELSQ3osocbXnShRZerbxJ2ceh3Vw6ufdJXK99bdDBsm1sRnnfin3NGFYMu3blqReLy2DXLDcgC33k1NfHsTbCuS")]
        public async Task Invalid_UpdateForm__Throws_ArgumentException(string newCategory, string newDescription)
        {
            ResourceUpdateForm form = new ResourceUpdateForm(newCategory, newDescription);

            ObjectResult result = await _sut.Edit(ResourceData.RESOURCE_01_CODE, form) as ObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData("unexisting1")]
        [InlineData("unexisting2")]
        [InlineData("unexisting3")]
        public async Task UnexistingCode__404NotFound(string resourceCode)
        {
            ResourceUpdateForm form = new ResourceUpdateForm("any", "any");

            var result = await _sut.Edit(resourceCode, form);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NullCode__Throws_ArgumentNullException()
        {
            ResourceUpdateForm form = new ResourceUpdateForm("any", "any");
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _sut.Edit(null, form);
            });
        }
    }
}
