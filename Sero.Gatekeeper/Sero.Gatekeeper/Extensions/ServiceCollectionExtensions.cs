using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGatekeeper(this IServiceCollection services)
        {
            services.TryAddScoped<AuthorizationContext>();
            services.TryAddScoped<IHateoasAuthorizator, DummyAuthorizationService>();

            services.AddSingleton<IValidateOptions<GtkOptions>, GtkOptionsValidator>();
        }
    }
}
