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
    public class ResourcesController : BaseHateoasController
    {
        public readonly IResourceStore ResourceStore;

        public ResourcesController(
            IResourceStore resourceStore)
        {
            this.ResourceStore = resourceStore;
        }

        [HttpGet("api/doorman/admin/resources")]
        [DoormanEndpoint(Constants.ResourceCodes.Resources, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] ResourceFilter filter)
        {
            var validationResult = new ResourcesFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await ResourceStore.Get(filter);

            if (page.IsEmpty)
                return NotFound();

            return Collection<Resource>(filter, page.Total, page.Items);
        }

        [HttpGet("api/doorman/admin/resources/{code}")]
        [Getter("resource")]
        [DoormanEndpoint(Constants.ResourceCodes.Resources, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCode([GetterParameter] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var resource = await ResourceStore.Get(code);

            if (resource == null)
                return NotFound();

            return Element<Resource>(resource);
        }

        [HttpPut("api/doorman/admin/resources/{code}")]
        [DoormanEndpoint(Constants.ResourceCodes.Resources, PermissionLevel.Write, EndpointScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] ResourceUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await ResourceStore.IsExisting(code))
                return NotFound();

            var validationResult = new ResourceUpdateFormValidator().Validate(form);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            Resource resource = await ResourceStore.Get(code);
            resource.Category = form.Category;
            resource.Description = form.Description;

            return AcceptedAtAction(nameof(GetByCode), new { resource.Code }, resource);
        }
    }
}
