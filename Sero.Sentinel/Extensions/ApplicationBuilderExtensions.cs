using Microsoft.AspNetCore.Builder;
using Sero.Sentinel.Storage;
using Sero.Sentinel.Storage.Seeders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Sentinel
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSentinel(this IApplicationBuilder app)
        {
            ICredentialRoleStore credentialRoleStore = (ICredentialRoleStore)app.ApplicationServices.GetService(typeof(ICredentialRoleStore));
            ICredentialStore credentialStore = (ICredentialStore)app.ApplicationServices.GetService(typeof(ICredentialStore));

            var credentialRoleSeeder = new CredentialRoleSeeder();
            var credentialSeeder = new CredentialSeeder();

            credentialRoleSeeder.Seed(credentialRoleStore).Wait();
            credentialSeeder.Seed(credentialRoleStore, credentialStore).Wait();

            //app.UseMiddleware<GatekeeperAuthenticationMiddleware>();
        }
    }
}
