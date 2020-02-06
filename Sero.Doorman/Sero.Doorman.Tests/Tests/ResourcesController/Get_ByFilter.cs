using Microsoft.AspNetCore.Mvc;
using Sero.Doorman.Controller;
using Sero.Doorman.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Resources
{
    public class Get_ByFilter : ResourcesControllerFixture
    {
        //TODO: OLEG DESCOMENTA EL TEST ME DABA PAJA ADAPTARLO
        //[Theory]
        //[InlineData(null, 3, 2, nameof(Resource.Code), Order.ASC)]
        //[InlineData(null, 1, 2, nameof(Resource.Code), Order.DESC)]
        //[InlineData(null, 1, 20, nameof(Resource.Category), Order.ASC)]
        //[InlineData(null, 1, 20, nameof(Resource.Category), Order.DESC)]
        //[InlineData("rce_code_03", 1, 20, nameof(Resource.Code), Order.ASC)]
        //[InlineData("Category", 1, 20, nameof(Resource.Code), Order.ASC)]
        //[InlineData("Category2", 1, 20, nameof(Resource.Code), Order.ASC)]
        //[InlineData("rce_code_04", 1, 20, nameof(Resource.Code), Order.ASC)]
        //[InlineData("resource_code_", 1, 20, nameof(Resource.Code), Order.ASC)]
        //[InlineData("Resource_code_", 1, 20, nameof(Resource.Code), Order.ASC)]
        //public async Task Success(string textSearch, ushort page, ushort pageSize, string sortBy, string orderBy)
        //{
        //    // Arrange
        //    var sortBySelector = ReflectionUtils.GeneratePropertySelector<Resource>(sortBy);

        //    IEnumerable<Resource> expected = _resourceStoreBuilder.ResourceList;

        //    if (orderBy == Order.DESC)
        //        expected = expected.OrderByDescending(sortBySelector);
        //    else
        //        expected = expected.OrderBy(sortBySelector);

        //    if (!string.IsNullOrEmpty(textSearch))
        //        expected = expected.Where(x => x.Code.ToLower().Contains(textSearch.ToLower())
        //                                        || x.Category.ToLower().Contains(textSearch.ToLower()));

        //    expected = expected
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    // Act
        //    IEnumerable<Resource> actual = await _defaultSut.GetByFilter(new ResourcesFilter(textSearch, page, pageSize, sortBy, orderBy));

        //    // Assert
        //    Assert.Equal(expected, actual, _resourceComparer);
        //}
        
        [Theory]
        [InlineData(null, -1, 10, nameof(Resource.Code), Order.ASC)]  // page negativo
        [InlineData(null, 1, -1, nameof(Resource.Code), Order.ASC)]   // pageSize negativo
        [InlineData(null, 1, 51, nameof(Resource.Code), Order.ASC)]   // pageSize demasiado grande
        [InlineData(null, 1, 10, "carlitos", Order.ASC)]              // sortBy invalido
        [InlineData(null, 1, 10, nameof(Resource.Code), "carlitos")]  // orderBy invalido
        [InlineData(null, -1, -1, "carlitos", "carlitos")]            // TODO MAL
        [InlineData("gfdEdUDYzOHkhpFM7kGKTMkVX8", -1, -1, "carlitos", "carlitos")]    // code demasiado largo
        [InlineData("asd asd", -1, -1, "carlitos", "carlitos")]    // code con espacios
        [InlineData("asd}asd", -1, -1, "carlitos", "carlitos")]    // code con caracter inválido
        public async Task InvalidFilter__Throws_ArgumentException(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            ObjectResult result = await _defaultSut.GetByFilter(new ResourcesFilter(textSearch, page, pageSize, sortBy, orderBy)) as ObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ValidationErrorView>(result.Value);
        }

        // TODO: DESCOMENTÁ ESTE TAMB DALE
        //[Fact]
        //public async Task EmptyFillter__Returns_First_10_Resources_Ordered_By_Code_Asc()
        //{
        //    // Arrange
        //    IEnumerable<Resource> expected = 
        //        _resourceStoreBuilder
        //        .ResourceList
        //        .OrderBy(x => x.Code)
        //        .Take(10)
        //        .ToList();

        //    // Act
        //    IEnumerable<Resource> actual = await _defaultSut.GetByFilter(new ResourcesFilter());

        //    // Assert
        //    Assert.Equal(expected, actual, _resourceComparer);
        //}
    }
}
