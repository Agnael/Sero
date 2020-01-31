using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sero.Doorman.Validators;

namespace Sero.Doorman.Controller
{
    [ApiController]
    [Route("api/doorman/admin/[controller]")]
    public class ResourcesController : ControllerBase
    {
        public readonly IResourceStore ResourceStore;

        public ResourcesController(IResourceStore resourceStore)
        {
            this.ResourceStore = resourceStore;
        }
        
        [HttpGet]
        //[RequirePermission(Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly)]
        [DoormanAction("asd", Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly, ActionScope.Collection)]
        public async Task<IEnumerable<Resource>> GetByFilter([FromQuery] ResourcesFilter filter)
        {
            var validationResult = new ResourcesFilterValidator().Validate(filter);

            if (!validationResult.IsValid)
                throw new ArgumentException();

            var resources = await ResourceStore.FetchAsync(filter);
            return resources;
        }

        [HttpGet]
        //[RequirePermission(Constants.ResourceCodes.Resources, PermissionLevel.ReadOnly)]
        [Route("{resourceCode}")]
        [DoormanAction("asd42", Constants.ResourceCodes.Resources, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<Resource> GetByCode(string resourceCode)
        {
            if (string.IsNullOrEmpty(resourceCode))
                throw new ArgumentNullException(nameof(resourceCode));

            var resource = await ResourceStore.FetchAsync(resourceCode);
            return resource;
        }

        [HttpPut]
        //[RequirePermission(Constants.ResourceCodes.Resources, PermissionLevel.ReadWrite)]
        [Route("{resourceCode}")]
        [DoormanAction("asd4", Constants.ResourceCodes.Resources, PermissionLevel.ReadWrite, ActionScope.Element)]
        public async Task<IActionResult> Update([FromRoute] string resourceCode,
                                            [FromBody] ResourceUpdateForm form)
        {
            if (string.IsNullOrEmpty(resourceCode))
                throw new ArgumentNullException(nameof(resourceCode));

            if (!await ResourceStore.IsExistingAsync(resourceCode))
                throw new ArgumentException("Unexisting resourceCode");

            var validationResult = new ResourceUpdateFormValidator().Validate(form);
            if (!validationResult.IsValid)
                throw new ArgumentException("Invalid update form");

            Resource resource = await ResourceStore.FetchAsync(resourceCode);
            resource.Category = form.Category;
            resource.Description = form.Description;

            await ResourceStore.UpdateAsync(resource);
            return StatusCode((int)HttpStatusCode.Accepted);
        }
    }
}
