using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;
using Sero.Doorman.Validators;
using Sero.Doorman.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Controller
{
    [ApiController]
    public class CredentialRolesController : HateoasController
    {
        public readonly IRoleStore RoleStore;
        public readonly ICredentialStore CredentialStore;

        public CredentialRolesController(
            IRoleStore roleStore,
            ICredentialStore credentialStore)
        {
            this.RoleStore = roleStore;
            this.CredentialStore = credentialStore;
        }

        //[HttpGet("api/doorman/admin/credentials")]
        //[DoormanAction(Constants.ResourceCodes.CredentialRoles, PermissionLevel.ReadOnly, EndpointScope.Collection)]
        //public async Task<IActionResult> GetByFilter([FromQuery] CredentialsFilter filter)
        //{
        //    var validationResult = new CredentialsFilterValidator().Validate(filter);
        //    validationResult.AddToModelState(this.ModelState, null);

        //    if (!validationResult.IsValid)
        //        return ValidationError();

        //    int credentialsTotal = await CredentialStore.CountAsync(filter);
        //    var credentials = await CredentialStore.FetchAsync(filter);

        //    if (credentials == null || credentials.Count() == 0)
        //        return NotFound();

        //    var vmList = new List<CredentialViewModel>();
        //    foreach(Credential credential in credentials)
        //    {
        //        var newVm = new CredentialViewModel(credential);
        //        vmList.Add(newVm);
        //    }

        //    return Collection(filter, credentialsTotal, vmList);
        //}
    }
}
