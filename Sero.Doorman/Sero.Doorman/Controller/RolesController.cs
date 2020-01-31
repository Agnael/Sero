using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Core.Mvc;
using Sero.Doorman.Validators;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    //[Route("api/doorman/admin/[controller]")]
    public class RolesController : DoormanController<Role>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RolesController(
            RequestUtils requestUtils,
            IRoleStore roleStore,
            IResourceStore resourceStore)
            : base (requestUtils)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;
        }

        [HttpGet("api/doorman/admin/roles")]
        [DoormanAction("role_get_byfilter", Constants.ResourceCodes.Roles, PermissionLevel.ReadOnly, ActionScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] RolesFilter filter)
        {
            var validationResult = new RolesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return await this.ValidationErrorAsync();

            int resourcesTotal = await RoleStore.CountAsync(filter);
            var resources = await RoleStore.FetchAsync(filter);

            var view = await this.SuccessAsync(resources, resourcesTotal);
            return view;
        }

        [HttpGet("api/doorman/admin/roles/{roleCode}")]
        //[Route("{roleCode}")]
        [DoormanAction("role_get_bycode", Constants.ResourceCodes.Roles, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> GetByCode(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode)) throw new ArgumentNullException(nameof(roleCode));

            var role = await RoleStore.FetchAsync(roleCode);

            var resultView = await this.SuccessAsync(role);
            return resultView;
        }

        [HttpPost("api/doorman/admin/roles")]
        [DoormanAction("role_create", Constants.ResourceCodes.Roles, PermissionLevel.ReadWrite, ActionScope.Collection)]
        public async Task<IActionResult> Create(RoleCreateForm form)
        {
            var validationResult = new RoleCreateFormValidator(RoleStore, ResourceStore).Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return await this.ValidationErrorAsync();

            Role role = new Role(form.Code, form.Name, form.Description, form.Permissions);
            await RoleStore.CreateAsync(role);

            var view = await this.SuccessAsync(role);
            this.SetStatusCode(StatusCodes.Status201Created);
            return view;
        }
    }
}
