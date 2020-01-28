using Microsoft.AspNetCore.Builder;
using Sero.Doorman.Middleware;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseDoorman(this IApplicationBuilder app)
        {
            app.UseMiddleware<DoormanAuthenticationMiddleware>();
        }
    }
}
