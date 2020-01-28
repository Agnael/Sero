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
using Sero.Doorman;
using Sero.Doorman.Stores;
using Sero.Doorman.Middleware;
using Serilog;
using Sero.Loxy;
using Newtonsoft.Json;
using Serilog.Filters;
using WebTest.Models;
using WebTest.Services;

namespace WebTest
{
    public class Startup
    {
        //public readonly IList<InMemoryCredential> TestCredentialList = new List<InMemoryCredential> {
        //    new InMemoryCredential
        //    {
        //        Id = 1,
        //        Email = "admin@mail.com",
        //        Password = "admin",
        //        Roles = new List<string>
        //        {
        //            "DefaultRole",
        //            "Admin"
        //        }
        //    },
        //    new InMemoryCredential
        //    {
        //        Id = 1,
        //        Email = "user@mail.com",
        //        Password = "user",
        //        Roles = new List<string>
        //        {
        //            "DefaultRole"
        //        }
        //    }
        //};

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
                    .WithMinimumLevel(LogLevel.Debug)
                    .WithExtendedLevel(LogLevel.Error)
                    .WithJsonFormatting(Formatting.Indented);

            services.Configure<DoormanOptions>(config => 
            {
                config.WithSecurityKey("j3Kkms555Msmkekwau3IsSJJJSKA3geLOa3AgnAEL83a");
                config.WithIssuer("olegsito");
            });

            //services.AddScoped<ICredentialStore>(_ => new InMemoryCredentialStore(TestCredentialList));
            //services.AddScoped<ICredentialService, CredentialService>();
            services.AddScoped<ICustomUserService, CustomUserService>();
            //services.AddScoped<IPermissionStore>(_ => new InMemoryPermissionStore(TestPermissionList));
            services.AddSingleton<IResourceStore>(_ => new InMemoryResourceStore(GetTestResources()));
            services.AddSingleton<IRoleStore>(_ => new InMemoryRoleStore(GetTestRoles()));

            services.AddControllers(conf => conf.Filters.Add<DoormanAuthorizationFilter>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseLoxy();
            app.UseDoorman();
            app.UseHttpsRedirection();
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

        public List<Role> GetTestRoles()
        {
            return new List<Role>
            {
                new Role(
                    "dm_role_admin",
                    "Doorman administrator",
                    "Has access to any doorman endpoint. Can manage any security aspect of the application.",
                    new Permission("dm_res_resources", PermissionLevel.ReadWrite),
                    new Permission("dm_res_roles", PermissionLevel.ReadWrite))
            };
        }

        public List<Resource> GetTestResources()
        {
            return new List<Resource>
            {
                new Resource("Sero.Doorman", "dm_res_resources", "Doorman resources administration."),
                new Resource("Sero.Doorman", "dm_res_roles", "Doorman role administration."),
            };
        }
    }
}
