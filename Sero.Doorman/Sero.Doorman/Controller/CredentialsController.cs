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

        [HttpGet("api/doorman/admin/credentials/{credentialId}")]
        [ElementGetter("credential")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.ReadOnly, EndpointScope.Element, EndpointRelation.Parent)]
        public async Task<IActionResult> GetByCredentialId([FromRoute] Guid? credentialId)
        {
            if (!credentialId.HasValue)
                return NotFound();

            Credential credential = await this.CredentialStore.FetchAsync(credentialId.Value);

            if (credential == null)
                return NotFound();

            CredentialViewModel vm = new CredentialViewModel(credential);
            return Element(vm);
        }

        [HttpGet("api/doorman/admin/credentials")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.ReadOnly, EndpointScope.Collection, EndpointRelation.Parent)]
        public async Task<IActionResult> GetByFilter([FromQuery] CredentialsFilter filter)
        {
            var validationResult = new CredentialsFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            int credentialsTotal = await CredentialStore.CountAsync(filter);
            var credentials = await CredentialStore.FetchAsync(filter);

            if (credentials == null || credentials.Count() == 0)
                return NotFound();

            var vmList = new List<CredentialViewModel>();
            foreach (Credential credential in credentials)
            {
                var newVm = new CredentialViewModel(credential);
                vmList.Add(newVm);
            }

            return Collection(filter, credentialsTotal, vmList);
        }

        [HttpGet("api/doorman/admin/credentials/{credentialId}/roles")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.ReadOnly, EndpointScope.Element, EndpointRelation.Child)]
        public async Task<IActionResult> Roles(Guid? credentialId)
        {
            if (!credentialId.HasValue)
                return NotFound();

            Credential credential = await CredentialStore.FetchAsync(credentialId.Value);

            if (credential == null)
                return NotFound();

            return Element(credential.Roles);
        }
    }
}
