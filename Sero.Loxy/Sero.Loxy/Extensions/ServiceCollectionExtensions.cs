using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sero.Loxy.Abstractions;
using Sero.Core;

namespace Sero.Loxy
{
    public static class ServiceCollectionExtensions
    {
        public static LoxyBuilder AddLoxy(this IServiceCollection services)
        {
            services.AddAppInfo();
            services.AddRequestInfo();

            var builder = new LoxyBuilder(services);

            services.TryAddScoped<LoxyMiddleware>();

            services.TryAddScoped<ILoxy>(serviceProvider => 
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                var appInfoService = serviceProvider.GetService<IAppInfoService>();
                var requestInfoService = serviceProvider.GetService<IRequestInfoService>();

                Loxy instance = new Loxy(httpContextAccessor, appInfoService, requestInfoService, builder);
                return instance;
            });

            return builder;
        }
    }
}
