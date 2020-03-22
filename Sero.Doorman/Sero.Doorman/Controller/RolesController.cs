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
    //[Route("[controller]")]
    public class RolesController : HateoasController
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

        //[DoormanEndpoint(Constants.ResourceCodes.Roles, PermissionLevel.Read, EndpointScope.Collection, EndpointRelation.Parent)]
        //public async Task<IActionResult> Test()
        //{
        //    return Ok("Oliiiixx");
        //}

        [HttpGet("api/doorman/admin/roles")]
        [DoormanEndpoint(Constants.ResourceCodes.Roles, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] RolesFilter filter)
        {
            var validationResult = new RolesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await RoleStore.Get(filter);

            if (page.IsEmpty)
                return NotFound();

            return Collection<Role>(filter, page.Total, page.Items);
        }

        [HttpGet("api/doorman/admin/roles/{code}")]
        [Getter("role")]
        [DoormanEndpoint(Constants.ResourceCodes.Roles, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCode([GetterParameter] string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code));

            var role = await RoleStore.Get(code);

            if (role == null)
                return NotFound();

            return Element<Role>(role);
        }

        [HttpPost("api/doorman/admin/roles")]
        [DoormanEndpoint(Constants.ResourceCodes.Roles, PermissionLevel.Write, EndpointScope.Collection)]
        public async Task<IActionResult> Create(RoleCreateForm form)
        {
            var validationResult = new RoleCreateFormValidator(RoleStore, ResourceStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = new Role(form.Code, form.Name, form.Description, form.Permissions);
            await RoleStore.Create(role);

            string url = Url.Action(nameof(GetByCode), new { role.Code });
            return Created(url, role);
        }

        [HttpPut("api/doorman/admin/roles/{code}")]
        [DoormanEndpoint(Constants.ResourceCodes.Roles, PermissionLevel.Write, EndpointScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] RoleUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await RoleStore.IsUnique(code))
                return NotFound();

            var validationResult = new RoleUpdateFormValidator(RoleStore, ResourceStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Role role = await RoleStore.Get(code);
            role.DisplayName = form.DisplayName;
            role.Description = form.Description;
            role.Permissions = form.Permissions;

            await RoleStore.Update(role);

            string getterUrl = Url.Action(nameof(GetByCode), new { role.Code });
            return Accepted(getterUrl, role);
        }
    }
}
