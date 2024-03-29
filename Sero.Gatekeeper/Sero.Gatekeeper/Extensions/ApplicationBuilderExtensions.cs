﻿using Microsoft.AspNetCore.Builder;
using Sero.Core;
using Sero.Gatekeeper.Storage;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Gatekeeper
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseGatekeeper(this IApplicationBuilder app)
        {
            HateoasService service = (HateoasService)app.ApplicationServices.GetService(typeof(HateoasService));
            IResourceStore resourceStore = (IResourceStore)app.ApplicationServices.GetService(typeof(IResourceStore));
            IRoleStore roleStore = (IRoleStore)app.ApplicationServices.GetService(typeof(IRoleStore));
            ICredentialStore credentialStore = (ICredentialStore)app.ApplicationServices.GetService(typeof(ICredentialStore));
            
            // Crea los registros básicos que necesita el sistema para funcionar
            var roleSeeder = new RoleSeeder();
            //var credentialSeeder = new CredentialSeeder();

            roleSeeder.Seed(resourceStore, roleStore).Wait();
            //credentialSeeder.Seed(roleStore, credentialStore).Wait();

            //app.UseMiddleware<GatekeeperAuthenticationMiddleware>();
        }
    }
}
