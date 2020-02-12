using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sero.Doorman.Validators;
using FluentValidation.AspNetCore;
using Sero.Core;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class ResourcesController : HateoasController<Resource>
    {
        public readonly IResourceStore ResourceStore;

        public ResourcesController(
            IResourceStore resourceStore)
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
                return ValidationError();

            var resourcesTotal = await ResourceStore.CountAsync(filter);
            var resources = await ResourceStore.FetchAsync(filter);

            return Collection(filter, resourcesTotal, resources);
        }

        [HttpGet("api/doorman/admin/resources/{code}")]
        [ElementGetter]
        [DoormanAction(Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly, ActionScope.Element)]
        public async Task<IActionResult> GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var resource = await ResourceStore.FetchAsync(code);

            if (resource == null)
                return NotFound();

            return Element(resource);
        }

        [HttpPut("api/doorman/admin/resources/{code}")]
        [DoormanAction(Constants.ResourceCodes.Resources, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] ResourceUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await ResourceStore.IsExistingAsync(code))
                return NotFound();

            var validationResult = new ResourceUpdateFormValidator().Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Resource resource = await ResourceStore.FetchAsync(code);
            resource.Category = form.Category;
            resource.Description = form.Description;

            string getterUrl = Url.Action(nameof(GetByCode), new { resource.Code });
            return Accepted(getterUrl, resource);
        }
    }
}
