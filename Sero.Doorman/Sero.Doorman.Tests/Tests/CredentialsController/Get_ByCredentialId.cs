using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sero.Doorman.Tests.Controllers.Credentials
{
    public class Get_ByCredentialId : CredentialsControllerFixture
    {
        [Theory]
        [InlineData(Constants.CredentialIds.Admin)]
        public async Task Success(string credentialId)
        {
            // Arrange
            Credential credential = _credentialStore
                .Credentials
                .FirstOrDefault(x => x.CredentialId == credentialId);

            CredentialVM expected = new CredentialVM(credential);

            // Act
            IActionResult result = await _sut.GetByCredentialId(credentialId);
            CredentialVM actual = result.AsElementView<Credential, CredentialVM>();

            // Assert
            Assert.Equal(expected, actual, _credentialVmComparer);
        }

        [Fact]
        public async Task UnexistingCredentialId__NotFound()
        {
            string unexistingCredentialId = "randomStuff0";

            IActionResult result = await _sut.GetByCredentialId(unexistingCredentialId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task NullCredentialId__NotFound()
        {
            IActionResult result = await _sut.GetByCredentialId(null);
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
