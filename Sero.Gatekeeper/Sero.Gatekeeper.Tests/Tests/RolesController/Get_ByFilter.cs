using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Gatekeeper.Controller;
using Sero.Gatekeeper.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Gatekeeper.Tests.Controllers.Roles
{
    public class Get_ByFilter : RolesControllerFixture
    {
        // TODO: Descomentar este test y arreglarlo
       //[Theory]
       //[InlineData(null, 3, 2, RoleSorting.Code, Order.Asc)]
       //[InlineData(null, 1, 2, RoleSorting.Code, Order.Desc)]
       //[InlineData(null, 1, 20, RoleSorting.Description, Order.Asc)]
       //[InlineData(null, 1, 20, RoleSorting.Description, Order.Desc)]
       //[InlineData(null, 1, 1, RoleSorting.DisplayName, Order.Asc)]
       //[InlineData(null, 2, 1, RoleSorting.DisplayName, Order.Desc)]
       //[InlineData("rce_code_03", 1, 20, RoleSorting.Code, Order.Asc)]
       //[InlineData("Category", 1, 20, RoleSorting.Code, Order.Asc)]
       //[InlineData("Category2", 1, 20, RoleSorting.Code, Order.Asc)]
       //[InlineData("rce_code_04", 1, 20, RoleSorting.Code, Order.Asc)]
       //[InlineData("role_code_", 1, 20, RoleSorting.Code, Order.Asc)]
       //[InlineData("Role_code_", 1, 20, RoleSorting.Code, Order.Asc)]
       // public async Task Success(string textSearch, int page, int pageSize, RoleSorting sortBy, Order orderBy)
       // {
       //     // Arrange
       //     var sortBySelector = ReflectionUtils.GeneratePropertySelector<Role>(sortBy);

       //     IEnumerable<Role> expected = _roleStore.Roles;

       //     if (orderBy == Order.Desc)
       //         expected = expected.OrderByDescending(sortBySelector);
       //     else
       //         expected = expected.OrderBy(sortBySelector);

       //     if (!string.IsNullOrEmpty(textSearch))
       //         expected = expected.Where(x => x.Code.ToLower().Contains(textSearch.ToLower())
       //                                         || x.Description.ToLower().Contains(textSearch.ToLower())
       //                                         || x.DisplayName.ToLower().Contains(textSearch.ToLower()));

       //     expected = expected
       //         .Skip((page - 1) * pageSize)
       //         .Take(pageSize)
       //         .ToList();

       //     // Act
       //     var form = new RoleFilter(textSearch, page, pageSize, sortBy, orderBy);
       //     IActionResult actionResult = await _sut.GetByFilter(form);

       //     if(expected.Count() == 0)
       //     {
       //         Assert.IsType<NotFoundResult>(actionResult);
       //     }
       //     else
       //     {
       //         ObjectResult result = actionResult as ObjectResult;
       //         CollectionView collectionResult = result.Value as CollectionView;
       //         IEnumerable<Role> actual = collectionResult.ViewModels as IEnumerable<Role>;

       //         // Assert
       //         Assert.Equal(expected, actual, _roleComparer);
       //     }
       // }

        [Theory]
        [InlineData(null, -1, 10, RoleSorting.Code, Order.Asc)]  // page negativo
        [InlineData(null, 1, -1, RoleSorting.Code, Order.Asc)]   // pageSize negativo
        [InlineData(null, 1, 51, RoleSorting.Code, Order.Asc)]   // pageSize demasiado grande
        [InlineData(null, 1, 10, RoleSorting.UNDEFINED, Order.Asc)]              // sortBy invalido
        [InlineData(null, 1, 10, RoleSorting.Code, Order.UNDEFINED)]  // orderBy invalido
        [InlineData(null, -1, -1, RoleSorting.UNDEFINED, Order.UNDEFINED)]
        [InlineData("gfdEdUDYzOHkhpFM7kGKTMkVX8", -1, -1, RoleSorting.UNDEFINED, Order.UNDEFINED)]    // code demasiado largo
        [InlineData("asd asd", -1, -1, RoleSorting.UNDEFINED, Order.UNDEFINED)]    // code con espacios
        [InlineData("asd}asd", -1, -1, RoleSorting.UNDEFINED, Order.UNDEFINED)]    // code con caracter inválido
        public async Task InvalidFilter__ValidationError(string textSearch, int page, int pageSize, RoleSorting sortBy, Order orderBy)
        {
            var form = new RoleFilter(page, pageSize, sortBy, orderBy, textSearch);
            var actionResult = await _sut.GetByFilter(form);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task EmptyFillter__Returns_First_10_Roles_Ordered_By_Code_Asc()
        {
            // Arrange
            IEnumerable<Role> expected =
                _roleStore
                .Roles
                .OrderBy(x => x.Code)
                .Take(10)
                .ToList();

            // Act
            ObjectResult result = await _sut.GetByFilter(new RoleFilter()) as ObjectResult;
            CollectionView collectionResult = result.Value as CollectionView;
            IEnumerable<Role> actual = collectionResult.ViewModels as IEnumerable<Role>;

            // Assert
            Assert.Equal(expected, actual, _roleComparer);
        }
    }
}
