using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDoorman(this IServiceCollection services)
        {
            services.TryAddScoped<AuthorizationContext>();
            services.TryAddScoped<IAuthorizationService, AuthorizationService>();
        }
    }
}
