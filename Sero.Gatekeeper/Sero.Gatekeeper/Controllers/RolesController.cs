using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Gatekeeper.Storage;
using Sero.Gatekeeper.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Controller
{
    [ApiController]
    public class RolesController : BaseHateoasController
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;
        public readonly ICodeRule CodeRule;
        public readonly IDescriptionRule DescriptionRule;
        public readonly IDisplayNameRule DisplayNameRule;

        public RolesController(
            IRoleStore roleStore,
            IResourceStore resourceStore,
            ICodeRule codeRule,
            IDescriptionRule descriptionRule,
            IDisplayNameRule displayNameRule)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;
            this.CodeRule = codeRule;
            this.DescriptionRule = descriptionRule;
            this.DisplayNameRule = displayNameRule;
        }
        
        [HttpGet("api/gatekeeper/admin/roles")]
        [SafeEndpoint(GtkResourceCodes.Roles, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] RoleFilter filter)
        {
            var validationResult = new RoleFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await RoleStore.Get(filter);
            
            if (page.IsEmpty)
                return NotFound<Role>(filter, page.Total);

            return Collection<Role>(filter, page.Total, page.Items);
        }

        [HttpGet("api/gatekeeper/admin/roles/{code}")]
        [Getter("role")]
        [SafeEndpoint(GtkResourceCodes.Roles, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCode([GetterParameter] string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code));

            var role = await RoleStore.Get(code);

            if (role == null)
                return NotFound();

            return Element<Role>(role);
        }

        [HttpPost("api/gatekeeper/admin/roles")]
        [SafeEndpoint(GtkResourceCodes.Roles, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> Create(RoleCreateForm form)
        {
            var validationResult = 
                new RoleCreateFormValidator(
                    RoleStore, 
                    ResourceStore, 
                    CodeRule, 
                    DescriptionRule, 
                    DisplayNameRule)
                .Validate(form);

            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = new Role(form.Code, form.Name, form.Description, form.Permissions);
            await RoleStore.Create(role);

            var view = Element<Role>(role);
            return CreatedAtAction(nameof(GetByCode), new { role.Code }, view);
        }

        [HttpPut("api/gatekeeper/admin/roles/{code}")]
        [SafeEndpoint(GtkResourceCodes.Roles, PermissionLevel.Write, EndpointScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] RoleUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await RoleStore.IsExisting(code))
                return NotFound();

            var validationResult = 
                new RoleUpdateFormValidator(
                    RoleStore, 
                    ResourceStore, 
                    DisplayNameRule, 
                    DescriptionRule)
                .Validate(form);

            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = await RoleStore.Get(code);
            role.DisplayName = form.DisplayName;
            role.Description = form.Description;
            role.Permissions = form.Permissions;

            await RoleStore.Update(role);

            var view = Element<Role>(role);
            return AcceptedAtAction(nameof(GetByCode), new { role.Code }, view);
        }
    }
}
