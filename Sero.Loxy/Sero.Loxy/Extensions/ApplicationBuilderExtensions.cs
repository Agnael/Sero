using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Registers the EventLoggingMiddleware. It's mandatory to register it to use Sero.Loxy.
        /// IMPORTANT: It must be the first middleware of the pipeline.
        /// </summary>
        public static void UseLoxy(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoxyMiddleware>();
        }
    }
}
