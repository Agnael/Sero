using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sero.Doorman.Validators;
using FluentValidation.AspNetCore;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class ResourcesController : DoormanController<Resource>
    {
        public readonly IResourceStore ResourceStore;

        public ResourcesController(
            RequestUtils requestUtils,
            IResourceStore resourceStore)
            : base(requestUtils)
        {
            this.ResourceStore = resourceStore;
        }

        [HttpGet("api/doorman/admin/resources")]
        [DoormanAction(Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly, ActionScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] ResourcesFilter filter)
        {
            var validationResult = new ResourcesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return await this.ValidationErrorAsync();

            var resourcesTotal = await ResourceStore.CountAsync(filter);
            var resources = await ResourceStore.FetchAsync(filter);

            var view = await CollectionAsync(resources, resourcesTotal);
            return view;
        }

        [HttpGet("api/doorman/admin/resources/{code}")]
        [DoormanElementGetter]
        [DoormanAction(Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly, ActionScope.Element)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var resource = await ResourceStore.FetchAsync(code);

            if (resource == null)
                return NotFound();

            var view = await ElementAsync(resource);
            return view;
        }

        [HttpPut("api/doorman/admin/resources/{code}")]
        [DoormanAction(Constants.ResourceCodes.Resources, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] ResourceUpdateForm form)
        {
            //if (string.IsNullOrEmpty(code))
            //    throw new ArgumentNullException(nameof(code));

            //if (!await ResourceStore.IsExistingAsync(code))
            //    throw new ArgumentException("Unexisting resourceCode");

            var validationResult = new ResourceUpdateFormValidator().Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return await this.ValidationErrorAsync();

            Resource resource = await ResourceStore.FetchAsync(code);
            resource.Category = form.Category;
            resource.Description = form.Description;

            await ResourceStore.UpdateAsync(resource);
            return StatusCode((int)HttpStatusCode.Accepted);
        }
    }
}
