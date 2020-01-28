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
        // TODO: DESCOMENTÁ Y ARREGLÁ EL TEST, LO SAQUÉ PARA PROBAR ALGO DE HATEOAS
        //[Theory]
        //[InlineData(null, 3, 2, nameof(Role.Code), Order.ASC)]
        //[InlineData(null, 1, 2, nameof(Role.Code), Order.DESC)]
        //[InlineData(null, 1, 20, nameof(Role.Description), Order.ASC)]
        //[InlineData(null, 1, 20, nameof(Role.Description), Order.DESC)]
        //[InlineData(null, 1, 1, nameof(Role.DisplayName), Order.ASC)]
        //[InlineData(null, 2, 1, nameof(Role.DisplayName), Order.DESC)]
        //[InlineData("rce_code_03", 1, 20, nameof(Role.Code), Order.ASC)]
        //[InlineData("Category", 1, 20, nameof(Role.Code), Order.ASC)]
        //[InlineData("Category2", 1, 20, nameof(Role.Code), Order.ASC)]
        //[InlineData("rce_code_04", 1, 20, nameof(Role.Code), Order.ASC)]
        //[InlineData("role_code_", 1, 20, nameof(Role.Code), Order.ASC)]
        //[InlineData("Role_code_", 1, 20, nameof(Role.Code), Order.ASC)]
        //public async Task Success(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        //{
        //    // Arrange
        //    var sortBySelector = ReflectionUtils.GeneratePropertySelector<Role>(sortBy);

        //    IEnumerable<Role> expected = _roleStoreBuilder.RoleList;

        //    if (orderBy == Order.DESC)
        //        expected = expected.OrderByDescending(sortBySelector);
        //    else
        //        expected = expected.OrderBy(sortBySelector);

        //    if (!string.IsNullOrEmpty(textSearch))
        //        expected = expected.Where(x => x.Code.ToLower().Contains(textSearch.ToLower())
        //                                        || x.Description.ToLower().Contains(textSearch.ToLower())
        //                                        || x.DisplayName.ToLower().Contains(textSearch.ToLower()));

        //    expected = expected
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    // Act
        //    IEnumerable<Role> actual = await _defaultSut.GetByFilter(new RolesFilter(textSearch, page, pageSize, sortBy, orderBy));

        //    // Assert
        //    Assert.Equal(expected, actual, _roleComparer);
        //}
        
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
        public async Task InvalidFilter__Throws_ArgumentException(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _defaultSut.GetByFilter(new RolesFilter(textSearch, page, pageSize, sortBy, orderBy));
            });
        }

        // TODO: DESCOMENTÁ Y ARREGLÁ EL TEST, LO SAQUÉ PARA PROBAR ALGO DE HATEOAS
        //[Fact]
        //public async Task EmptyFillter__Returns_First_10_Roles_Ordered_By_Code_Asc()
        //{
        //    // Arrange
        //    IEnumerable<Role> expected = 
        //        _roleStoreBuilder
        //        .RoleList
        //        .OrderBy(x => x.Code)
        //        .Take(10)
        //        .ToList();

        //    // Act
        //    IEnumerable<Role> actual = await _defaultSut.GetByFilter(new RolesFilter());

        //    // Assert
        //    Assert.Equal(expected, actual, _roleComparer);
        //}
    }
}
