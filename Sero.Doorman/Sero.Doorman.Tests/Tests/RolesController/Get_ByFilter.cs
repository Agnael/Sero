using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Doorman.Controller;
using Sero.Doorman.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class Get_ByFilter : RolesControllerFixture
    {
       [Theory]
       [InlineData(null, 3, 2, nameof(Role.Code), Order.ASC)]
       [InlineData(null, 1, 2, nameof(Role.Code), Order.DESC)]
       [InlineData(null, 1, 20, nameof(Role.Description), Order.ASC)]
       [InlineData(null, 1, 20, nameof(Role.Description), Order.DESC)]
       [InlineData(null, 1, 1, nameof(Role.DisplayName), Order.ASC)]
       [InlineData(null, 2, 1, nameof(Role.DisplayName), Order.DESC)]
       [InlineData("rce_code_03", 1, 20, nameof(Role.Code), Order.ASC)]
       [InlineData("Category", 1, 20, nameof(Role.Code), Order.ASC)]
       [InlineData("Category2", 1, 20, nameof(Role.Code), Order.ASC)]
       [InlineData("rce_code_04", 1, 20, nameof(Role.Code), Order.ASC)]
       [InlineData("role_code_", 1, 20, nameof(Role.Code), Order.ASC)]
       [InlineData("Role_code_", 1, 20, nameof(Role.Code), Order.ASC)]
        public async Task Success(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            // Arrange
            var sortBySelector = ReflectionUtils.GeneratePropertySelector<Role>(sortBy);

            IEnumerable<Role> expected = _roleStoreBuilder.RoleList;

            if (orderBy == Order.DESC)
                expected = expected.OrderByDescending(sortBySelector);
            else
                expected = expected.OrderBy(sortBySelector);

            if (!string.IsNullOrEmpty(textSearch))
                expected = expected.Where(x => x.Code.ToLower().Contains(textSearch.ToLower())
                                                || x.Description.ToLower().Contains(textSearch.ToLower())
                                                || x.DisplayName.ToLower().Contains(textSearch.ToLower()));

            expected = expected
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Act
            var form = new RolesFilter(textSearch, page, pageSize, sortBy, orderBy);
            IActionResult actionResult = await _defaultSut.GetByFilter(form);

            if(expected.Count() == 0)
            {
                Assert.IsType<NotFoundResult>(actionResult);
            }
            else
            {
                ObjectResult result = actionResult as ObjectResult;
                CollectionResult collectionResult = result.Value as CollectionResult;
                IEnumerable<Role> actual = collectionResult.ElementsToReturn as IEnumerable<Role>;

                // Assert
                Assert.Equal(expected, actual, _roleComparer);
            }
        }

        [Theory]
        [InlineData(null, -1, 10, nameof(Role.Code), Order.ASC)]  // page negativo
        [InlineData(null, 1, -1, nameof(Role.Code), Order.ASC)]   // pageSize negativo
        [InlineData(null, 1, 51, nameof(Role.Code), Order.ASC)]   // pageSize demasiado grande
        [InlineData(null, 1, 10, "carlitos", Order.ASC)]              // sortBy invalido
        [InlineData(null, 1, 10, nameof(Role.Code), "carlitos")]  // orderBy invalido
        [InlineData(null, -1, -1, "carlitos", "carlitos")]            // TODO MAL
        [InlineData("gfdEdUDYzOHkhpFM7kGKTMkVX8", -1, -1, "carlitos", "carlitos")]    // code demasiado largo
        [InlineData("asd asd", -1, -1, "carlitos", "carlitos")]    // code con espacios
        [InlineData("asd}asd", -1, -1, "carlitos", "carlitos")]    // code con caracter inválido
        public async Task InvalidFilter__ValidationError(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            var form = new RolesFilter(textSearch, page, pageSize, sortBy, orderBy);
            ObjectResult result = await _defaultSut.GetByFilter(form) as ObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.IsType<ValidationErrorView>(result.Value);
        }

        [Fact]
        public async Task EmptyFillter__Returns_First_10_Roles_Ordered_By_Code_Asc()
        {
            // Arrange
            IEnumerable<Role> expected =
                _roleStoreBuilder
                .RoleList
                .OrderBy(x => x.Code)
                .Take(10)
                .ToList();

            // Act
            ObjectResult result = await _defaultSut.GetByFilter(new RolesFilter()) as ObjectResult;
            CollectionResult collectionResult = result.Value as CollectionResult;
            IEnumerable<Role> actual = collectionResult.ElementsToReturn as IEnumerable<Role>;

            // Assert
            Assert.Equal(expected, actual, _roleComparer);
        }
    }
}
