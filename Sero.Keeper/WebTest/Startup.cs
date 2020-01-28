using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Sero.Loxy;
using Serilog.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Filters;
using Sero.Mapper;
using WebTest.Mappings;

namespace WebTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSerilog();

            services.AddLogging(x => x.AddSerilog());

            services.AddLoxy()
                .Sinks.AddJsonSink()
                    .WithMinimumLevel(LogLevel.Information)
                    .WithExtendedLevel(LogLevel.Error)
                    .WithJsonFormatting(Formatting.Indented);

            services.AddScoped<AppKeeper>();
            services.AddScoped<UserStore>();

            services.AddSeroMapper(x => x.AddSheet<EntityToModelMappings>()
                                            .AddSheet<ModelToEntityMappings>());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseLoxy();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
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
