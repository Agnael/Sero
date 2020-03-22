using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Doorman.Validators;
using Sero.Doorman.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class CredentialsController : HateoasController
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;
        public readonly ICredentialStore CredentialStore;

        public CredentialsController(
            IRoleStore roleStore,
            IResourceStore resourceStore,
            ICredentialStore credentialStore)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;
            this.CredentialStore = credentialStore;
        }

        [HttpGet("api/doorman/admin/credentials/{username}")]
        [Getter("credential")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByUsername([GetterParameter][FromRoute] string username)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            Credential credential = await this.CredentialStore.Get(username);

            if (credential == null)
                return NotFound();

            CredentialViewModel vm = new CredentialViewModel(credential);
            return Element<Credential>(vm);
        }

        [HttpGet("api/doorman/admin/credentials")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] CredentialsFilter filter)
        {
            var validationResult = new CredentialsFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.Get(filter);

            if (page.IsEmpty)
                return NotFound();

            var vmList = new List<CredentialViewModel>();
            foreach (Credential credential in page.Items)
            {
                var newVm = new CredentialViewModel(credential);
                vmList.Add(newVm);
            }

            return Collection<Credential>(filter, page.Total, vmList);
        }

        [HttpGet("api/doorman/admin/credentials/{username}/roles")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> Roles([FromRoute] string username, [FromQuery] RolesFilter filter)
        {
            if (string.IsNullOrEmpty(username))
                return NotFound();

            var validationResult = new RolesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.GetRoles(username, filter);
            
            if (page.IsEmpty)
                return NotFound();

            return Collection<Role>(filter, page.Total, page.Items);
        }
    }
}
