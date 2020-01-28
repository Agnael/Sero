using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Filters;
using Sero.Loxy;
using Sero.Loxy.Abstractions;
using Sero.Loxy.EfCore;

namespace TestWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSerilog();

            services.AddLogging(x => x.AddSerilog());

            services.AddLoxy()
                .Sinks.AddJsonSink()
                    .WithMinimumLevel(LogLevel.Debug)
                    .WithExtendedLevel(LogLevel.Error)
                    .WithJsonFormatting(Formatting.Indented);

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseLoxy();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                            // IMPORTANT, clears all the extra logging messages that microsoft forces into the app:
                            .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                            .WriteTo.Console()
                            .MinimumLevel.Information()
                            .CreateLogger();
        }
    }
}
