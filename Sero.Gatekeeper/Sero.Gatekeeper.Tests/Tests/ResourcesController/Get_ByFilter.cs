using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Gatekeeper.Controller;
using Sero.Gatekeeper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Gatekeeper.Tests.Controllers.Resources
{
    public class Get_ByFilter : ResourcesControllerFixture
    {
        // TODO: Descomentar y arreglar test
        //[Theory]
        //[InlineData(null, 3, 2, ResourceSorting.Code, Order.Asc)]
        //[InlineData(null, 1, 2, ResourceSorting.Code, Order.Desc)]
        //[InlineData(null, 1, 20, ResourceSorting.Category, Order.Asc)]
        //[InlineData(null, 1, 20, ResourceSorting.Category, Order.Desc)]
        //[InlineData("rce_code_03", 1, 20, ResourceSorting.Code, Order.Asc)]
        //[InlineData("Category", 1, 20, ResourceSorting.Code, Order.Asc)]
        //[InlineData("Category2", 1, 20, ResourceSorting.Code, Order.Asc)]
        //[InlineData("rce_code_04", 1, 20, ResourceSorting.Code, Order.Asc)]
        //[InlineData("resource_code_", 1, 20, ResourceSorting.Code, Order.Asc)]
        //[InlineData("Resource_code_", 1, 20, ResourceSorting.Code, Order.Asc)]
        //public async Task Success(string textSearch, ushort page, ushort pageSize, ResourceSorting sortBy, Order orderBy)
        //{
        //    // Arrange
        //    var sortBySelector = ReflectionUtils.GeneratePropertySelector<Resource>(sortBy);

        //    IEnumerable<Resource> expected = _resourceStore.Resources;

        //    if (orderBy == Order.Desc)
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
        //    var form = new ResourceFilter(textSearch, page, pageSize, sortBy, orderBy);
        //    var result = await _sut.GetByFilter(form) as ObjectResult;
        //    var collectionResult = result.Value as CollectionView;
        //    IEnumerable<Resource> actual = collectionResult.ViewModels as IEnumerable<Resource>;

        //    // Assert
        //    Assert.Equal(expected, actual, _resourceComparer);
        //}

        [Theory]
        [InlineData(null, -1, 10, ResourceSorting.Code, Order.Asc)]  // page negativo
        [InlineData(null, 1, -1, ResourceSorting.Code, Order.Asc)]   // pageSize negativo
        [InlineData(null, 1, 51, ResourceSorting.Code, Order.Asc)]   // pageSize demasiado grande
        [InlineData(null, 1, 10, ResourceSorting.UNDEFINED, Order.Asc)]              // sortBy invalido
        [InlineData(null, 1, 10, ResourceSorting.Code, Order.UNDEFINED)]  // orderBy invalido
        [InlineData(null, -1, -1, ResourceSorting.UNDEFINED, Order.UNDEFINED)]
        [InlineData("gfdEdUDYzOHkhpFM7kGKTMkVX8", -1, -1, ResourceSorting.UNDEFINED, Order.UNDEFINED)]    // code demasiado largo
        [InlineData("asd asd", -1, -1, ResourceSorting.UNDEFINED, Order.UNDEFINED)]    // code con espacios
        [InlineData("asd}asd", -1, -1, ResourceSorting.UNDEFINED, Order.UNDEFINED)]    // code con caracter inválido
        public async Task InvalidFilter__Throws_ArgumentException(string textSearch, int page, int pageSize, ResourceSorting sortBy, Order orderBy)
        {
            ObjectResult result = await _sut.GetByFilter(new ResourceFilter(page, pageSize, sortBy, orderBy, textSearch)) as ObjectResult;

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task EmptyFillter__Returns_First_10_Resources_Ordered_By_Code_Asc()
        {
            // Arrange
            IEnumerable<Resource> expected =
                _resourceStore
                .Resources
                .OrderBy(x => x.Code)
                .Take(10)
                .ToList();

            // Act
            var result = await _sut.GetByFilter(new ResourceFilter()) as ObjectResult;
            var collectionResult = result.Value as CollectionView;
            IEnumerable<Resource> actual = collectionResult.ViewModels as IEnumerable<Resource>;

            // Assert
            Assert.Equal(expected, actual, _resourceComparer);
        }
    }
}
