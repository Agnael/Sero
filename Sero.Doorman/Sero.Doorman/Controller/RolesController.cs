using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Doorman.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class RolesController : HateoasController<Role>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RolesController(
            IRoleStore roleStore,
            IResourceStore resourceStore)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;
        }

        [HttpGet("api/doorman/admin/roles")]
        [DoormanAction(Constants.ResourceCodes.Roles, PermissionLevel.ReadOnly, ActionScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] RolesFilter filter)
        {
            var validationResult = new RolesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            int resourcesTotal = await RoleStore.CountAsync(filter);
            var resources = await RoleStore.FetchAsync(filter);

            if (resources == null || resources.Count == 0)
                return NotFound();

            return Collection(filter, resourcesTotal, resources);
        }

        [HttpGet("api/doorman/admin/roles/{code}")]
        [ElementGetter]
        [DoormanAction(Constants.ResourceCodes.Roles, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code));

            var role = await RoleStore.FetchAsync(code);

            if (role == null)
                return NotFound();

            return Element(role);
        }

        [HttpPost("api/doorman/admin/roles")]
        [DoormanAction(Constants.ResourceCodes.Roles, PermissionLevel.ReadWrite, ActionScope.Collection)]
        public async Task<IActionResult> Create(RoleCreateForm form)
        {
            var validationResult = new RoleCreateFormValidator(RoleStore, ResourceStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = new Role(form.Code, form.Name, form.Description, form.Permissions);
            await RoleStore.CreateAsync(role);

            string url = Url.Action(nameof(GetByCode), new { role.Code });
            return Created(url, role);
        }

        [HttpPut("api/doorman/admin/roles/{code}")]
        [DoormanAction(Constants.ResourceCodes.Roles, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] RoleUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await RoleStore.IsExistingAsync(code))
                return NotFound();

            var validationResult = new RoleUpdateFormValidator(RoleStore, ResourceStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = await RoleStore.FetchAsync(code);
            role.DisplayName = form.DisplayName;
            role.Description = form.Description;
            role.Permissions = form.Permissions;

            await RoleStore.UpdateAsync(role);

            string getterUrl = Url.Action(nameof(GetByCode), new { role.Code });
            return Accepted(getterUrl, role);
        }
    }
}
