using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Doorman.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class CredentialsController : BaseHateoasController
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

        [HttpPost("api/doorman/admin/credentials")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> Create(CredentialCreateForm form)
        {
            var validationResult = new CredentialCreateFormValidator(CredentialStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();
            
            Credential credential = new Credential();
            credential.BirthDate = form.Birthdate;
            credential.CreationDate = DateTime.UtcNow;
            credential.DisplayName = form.CredentialId;
            credential.CredentialId = form.CredentialId.ToLower();
            credential.Email = form.Email;
            credential.PasswordSalt = HashingUtil.GenerateSalt();
            credential.PasswordHash = HashingUtil.GenerateHash(form.Password, credential.PasswordSalt);

            Role defaultRole = await RoleStore.Get(Constants.RoleCodes.User);
            credential.Roles.Add(defaultRole);
            
            await this.CredentialStore.Create(credential);
            
            string url = Url.Action(nameof(GetByCredentialId), new { credential.CredentialId });
            var view = await this.GetByCredentialId(credential.CredentialId);

            return Created(url, view);
        }

        [HttpGet("api/doorman/admin/credentials/{CredentialId}")]
        [Getter("Credential")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCredentialId([GetterParameter][FromRoute] string CredentialId)
        {
            if (string.IsNullOrEmpty(CredentialId))
                return BadRequest();

            Credential credential = await this.CredentialStore.Get(CredentialId);

            if (credential == null)
                return NotFound();

            CredentialVM vm = new CredentialVM(credential);
            return Element<Credential>(vm);
        }

        [HttpGet("api/doorman/admin/credentials")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] CredentialFilter filter)
        {
            var validationResult = new CredentialsFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.Get(filter);

            if (page.IsEmpty)
                return NotFound();

            var vmList = new List<CredentialVM>();
            foreach (Credential credential in page.Items)
            {
                var newVm = new CredentialVM(credential);
                vmList.Add(newVm);
            }

            return Collection<Credential>(filter, page.Total, vmList);
        }

        [HttpGet("api/doorman/admin/credentials/{CredentialId}/roles")]
        [DoormanEndpoint(Constants.ResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> Roles([FromRoute] string CredentialId, [FromQuery] RoleFilter filter)
        {
            if (string.IsNullOrEmpty(CredentialId))
                return NotFound();

            var validationResult = new RolesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.GetRoles(CredentialId, filter);
            
            if (page.IsEmpty)
                return NotFound();

            return Collection<Role>(filter, page.Total, page.Items);
        }
    }
}
