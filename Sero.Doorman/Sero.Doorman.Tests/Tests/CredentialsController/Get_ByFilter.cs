using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Credentials
{
    public class Get_ByFilter : CredentialsControllerFixture
    {
        public static IEnumerable<object[]> Success_Data()
        {
            yield return
                new CredentialFilter(2, 2, CredentialSorting.CredentialId, Order.Asc, null, null, null, null, null, null, null)
                .ToArray();

            yield return
                new CredentialFilter(1, 2, CredentialSorting.CredentialId, Order.Desc, null, null, null, null, null, null, null)
                .ToArray();

            yield return
                new CredentialFilter(1, 20, CredentialSorting.BirthDate, Order.Asc, null, null, null, null, null, null, null)
                .ToArray();
            yield return
                new CredentialFilter(1, 15, CredentialSorting.BirthDate, Order.Desc, null, null, null, null, null, null, null)
                .ToArray();

            yield return
                new CredentialFilter(1, 1, CredentialSorting.CreationDate, Order.Asc, null, null, null, null, null, null, null)
                .ToArray();
            yield return
                new CredentialFilter(2, 1, CredentialSorting.CreationDate, Order.Desc, null, null, null, null, null, null, null)
                .ToArray();

            yield return
                new CredentialFilter(1, 3, CredentialSorting.Email, Order.Asc, null, null, null, null, null, null, null)
                .ToArray();
            yield return
                new CredentialFilter(8, 43, CredentialSorting.Email, Order.Desc, null, null, null, null, null, null, null)
                .ToArray();

            var roleList_1 = new List<string> { RoleData.Role_01_Admin.Code, Constants.RoleCodes.Admin };
            yield return
                new CredentialFilter(1, 10, CredentialSorting.CreationDate, Order.Desc, null, roleList_1, null, null, null, null, null)
                .ToArray();
        }

        [Theory]
        [MemberData(nameof(Success_Data))]
        public async Task Success(CredentialFilter filter)
        {
            // Arrange
            // TODO: No tiene demasiado sentido usar un InMemory para generar el expected, ya que tambien es un InMemory 
            // el que está dentro del SUT actual, pero la idea es que se puedan escribir implementaciones distintas y que 
            // cada una deba cumplir con este mismo test, y que si la implementación es de EF, entonces que use el InMemory 
            // como expected y el EF como actual. Ni idea de como conseguir eso, pero por ahora esto queda así.
            Page<Credential> page = await new InMemoryCredentialStore(_credentialStore.Credentials).Get(filter);
            IEnumerable<CredentialVM> expected = page.Items.Select(x => new CredentialVM(x));

            // Act
            IActionResult actionResult = await _sut.GetByFilter(filter);

            if (page.IsEmpty)
            {
                // Assert
                Assert.IsType<NotFoundResult>(actionResult);
            }
            else
            {
                var actual = actionResult.AsCollectionView<Credential, CredentialVM>();

                // Assert
                Assert.Equal(expected, actual, _credentialVmComparer);
            }
        }
    }
}
