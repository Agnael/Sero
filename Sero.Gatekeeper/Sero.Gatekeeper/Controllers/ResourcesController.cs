using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sero.Gatekeeper.Validators;
using FluentValidation.AspNetCore;
using Sero.Core;
using Sero.Gatekeeper.Storage;

namespace Sero.Gatekeeper.Controller
{
    [ApiController]
    public class ResourcesController : BaseHateoasController
    {
        public readonly IResourceStore ResourceStore;
        public readonly ICodeRule CodeRule;
        public readonly IDescriptionRule DescriptionRule;

        public ResourcesController(
            IResourceStore resourceStore,
            ICodeRule codeRule,
            IDescriptionRule descriptionRule)
        {
            this.ResourceStore = resourceStore;
            this.CodeRule = codeRule;
            this.DescriptionRule = descriptionRule;
        }

        [HttpGet("api/gatekeeper/admin/resources")]
        [SafeEndpoint(GtkResourceCodes.Resources, PermissionLevel.Read, EndpointScope.Collection)]
        public async Task<IActionResult> GetByFilter([FromQuery] ResourceFilter filter)
        {
            var validationResult = new ResourceFilterValidator().Validate(filter);
            validationResult.AddToModelState(this.ModelState, null);

            if (!validationResult.IsValid)
                return ValidationError();

            var page = await ResourceStore.Get(filter);

            if (page.IsEmpty)
                return NotFound<Resource>(filter, page.Total);

            return Collection<Resource>(filter, page.Total, page.Items);
        }

        [HttpGet("api/gatekeeper/admin/resources/{code}")]
        [Getter("resource")]
        [SafeEndpoint(GtkResourceCodes.Resources, PermissionLevel.Read, EndpointScope.Element)]
        public async Task<IActionResult> GetByCode([GetterParameter] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest();

            var resource = await ResourceStore.Get(code);

            if (resource == null)
                return NotFound();

            return Element<Resource>(resource);
        }

        [HttpPut("api/gatekeeper/admin/resources/{code}")]
        [SafeEndpoint(GtkResourceCodes.Resources, PermissionLevel.Write, EndpointScope.Element)]
        public async Task<IActionResult> Edit(
            [FromRoute] string code,
            [FromBody] ResourceUpdateForm form)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            if (!await ResourceStore.IsExisting(code))
                return NotFound();

            var validationResult = 
                new ResourceUpdateFormValidator(
                    CodeRule, 
                    DescriptionRule)
                .Validate(form);

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
