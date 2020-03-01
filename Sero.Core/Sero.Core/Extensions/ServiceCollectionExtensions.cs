using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sero.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppInfo(this IServiceCollection services)
        {
            services.TryAddSingleton<IAppInfoService, AppInfoService>();
        }

        public static void AddRequestInfo(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IRequestInfoService, RequestInfoService>();
            services.TryAddSingleton<HateoasService>();
        }
    }
}
