using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Sero.Core;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sero.Sentinel
{
    [ApiController]
    public class CredentialsController : BaseHateoasController
    {
        public readonly ICredentialRoleStore CredentialRoleStore;
        public readonly ICredentialStore CredentialStore;
        public readonly IDisplayNameRule DisplayNameRule;

        public CredentialsController(
            ICredentialRoleStore credentialRoleStore,
            ICredentialStore credentialStore,
            IDisplayNameRule displayNameRule)
        {
            this.CredentialRoleStore = credentialRoleStore;
            this.CredentialStore = credentialStore;
            this.DisplayNameRule = displayNameRule;
        }

        [HttpPost("api/sentinel/credentials")]
        [SafeEndpoint(SentinelResourceCodes.Credentials, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> Create(CredentialCreateForm form)
        {
            var validationResult = 
                new CredentialCreateFormValidator(
                    CredentialStore, 
                    DisplayNameRule)
                .Validate(form);

            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();
            
            Credential credential = new Credential();
            credential.BirthDate = form.Birthdate;
            credential.CreationDate = DateTime.UtcNow;
            credential.DisplayName = form.Username;
            credential.CredentialId = form.Username.ToLower();
            credential.Email = form.Email;
            credential.PasswordSalt = HashingUtil.GenerateSalt();
            credential.PasswordHash = HashingUtil.GenerateHash(form.Password, credential.PasswordSalt);

            CredentialRole defaultRole = await CredentialRoleStore.Get(SentinelCredentialRoleCodes.RegularUser);
            credential.Roles.Add(defaultRole);
            
            await this.CredentialStore.Create(credential);
            
            string url = Url.Action(nameof(GetByCredentialId), new { credential.CredentialId });
            var view = await this.GetByCredentialId(credential.CredentialId);

            return Created(url, view);
        }

        [HttpGet("api/sentinel/credentials/{credentialId}")]
        [Getter("Credential")]
        [SafeEndpoint(SentinelResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCredentialId([GetterParameter][FromRoute] string credentialId)
        {
            if (string.IsNullOrEmpty(credentialId))
                return BadRequest();

            Credential credential = await this.CredentialStore.Get(credentialId);

            if (credential == null)
                return NotFound();

            CredentialVM vm = new CredentialVM(credential);
            return Element<Credential>(vm);
        }

        [HttpGet("api/sentinel/credentials")]
        [SafeEndpoint(SentinelResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] CredentialFilter filter)
        {
            var validationResult = new CredentialFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.Get(filter);

            if (page.IsEmpty)
                return NotFound<Credential>(filter, page.Total);

            var vmList = new List<CredentialVM>();
            foreach (Credential credential in page.Items)
            {
                var newVm = new CredentialVM(credential);
                vmList.Add(newVm);
            }

            return Collection<Credential>(filter, page.Total, vmList);
        }

        [HttpGet("api/sentinel/credentials/{CredentialId}/roles")]
        [SafeEndpoint(SentinelResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> Roles([FromRoute] string credentialId, [FromQuery] CredentialRoleFilter filter)
        {
            if (string.IsNullOrEmpty(credentialId))
                return NotFound();

            var validationResult = new CredentialRoleFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await CredentialStore.GetRoles(credentialId, filter);
            
            if (page.IsEmpty)
                return NotFound<CredentialRole>(filter, page);

            return Collection<CredentialRole>(filter, page);
        }

        [HttpGet("api/sentinel/credentials/{credentialId}/sessions")]
        [SafeEndpoint(SentinelResourceCodes.Credentials, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> Sessions([FromRoute] string credentialId, [FromQuery] SessionFilter filter)
        {
            if(string.IsNullOrEmpty(credentialId))
                return NotFound();

            var validationResult = new SessionFilterValidator(DisplayNameRule).Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var sessionsPage = await CredentialStore.GetSessions(credentialId, filter);

            if (sessionsPage.IsEmpty)
                return NotFound<Session>(filter, sessionsPage);

            return Collection<Session>(filter, sessionsPage);
        }
    }
}
