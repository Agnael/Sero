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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sero.Core;

namespace WebTest
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

            services.Configure<DoormanOptions>(config => 
            {
                config.WithSecurityKey("j3Kkms555Msmkekwau3IsSJJJSKA3geLOa3AgnAEL83a");
                config.WithIssuer("olegsito");
            });

            services.AddSingleton<IResourceStore>(_ => new InMemoryResourceStore(GetTestResources()));
            services.AddSingleton<IRoleStore>(_ => new InMemoryRoleStore(GetTestRoles()));
            services.AddSingleton<ICredentialStore>(_ => new InMemoryCredentialStore(GetTestCredentials()));
            services.AddDoorman();

            services.AddControllers(conf => {
                conf.Filters.Add<DoormanFilter>();
                conf.Filters.Add<HateoasFilter>();
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
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

                //endpoints.MapControllerRoute(
                //        "test",
                //        "/",
                //        new { controller = "Roles", action = "Test" })
                //    .WithMetadata(new HttpGetAttribute());
            });

            Doorman.HealthCheck(actionDescriptorCollectionProvider);
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

        public List<Credential> GetTestCredentials()
        {
            var testRoles = GetTestRoles();

            var credentials = new List<Credential>();

            string newId = "";
            
            Credential cred1 = new Credential();
            //cred1.CredentialId = new Guid("60530e59-a921-4ba5-89bb-6689446c4468");
            cred1.Username = "agnael";
            cred1.DisplayName = "Agnael";
            cred1.CreationDate = new DateTime(2015, 5, 4);
            cred1.Email = "oleg.kuzmych@nitra.com";
            cred1.PasswordSalt = "ABCDEFGHIJKLMNOP";
            cred1.PasswordHash = HashingUtil.GenerateHash("password123", cred1.PasswordSalt);
            cred1.BirthDate = new DateTime(1994, 10, 26);
            cred1.Roles = new List<Role>
            {
                testRoles[0],
                testRoles[1],
                testRoles[2],
                testRoles[3]
            };

            Credential cred2 = new Credential();
            //cred2.Username = new Guid("ac28ae7e-b85c-41e2-9813-31f9f9b12384");
            cred2.Username = "simbad";
            cred2.DisplayName = "Simbad";
            cred2.CreationDate = new DateTime(2015, 5, 4);
            cred2.Email = "random.pibito@gmail.com";
            cred2.PasswordSalt = "ABCDEFGHIJKLMNOP";
            cred2.PasswordHash = HashingUtil.GenerateHash("password321", cred1.PasswordSalt);
            cred2.BirthDate = new DateTime(1998, 6, 2);
            cred2.Roles = new List<Role>
            {
                testRoles[1],
                testRoles[3]
            };

            credentials.Add(cred1);
            credentials.Add(cred2);

            return credentials;
        }

        public List<Role> GetTestRoles()
        {
            return new List<Role>
            {
                new Role(
                    "dm_role_admin01",
                    "Doorman administrator",
                    "Has access to any doorman endpoint. Can manage any security aspect of the application.",
                    new Permission("dm_res_resources", PermissionLevel.Write),
                    new Permission("dm_res_roles", PermissionLevel.Write)),
                new Role(
                    "dm_role_admin02",
                    "TEST02",
                    "DESC02",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin03",
                    "TEST03",
                    "DESC03",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin04",
                    "TEST04",
                    "DESC04",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin05",
                    "TEST05",
                    "DESC05",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin06",
                    "TEST06",
                    "DESC06",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin07",
                    "TEST07",
                    "DESC07",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin08",
                    "TEST08",
                    "DESC08",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin09",
                    "TEST09",
                    "DESC09",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin10",
                    "TEST10",
                    "DESC10",
                    new Permission("dm_res_resources", PermissionLevel.Read)),
                new Role(
                    "dm_role_admin11",
                    "TEST11",
                    "DESC11",
                    new Permission("dm_res_resources", PermissionLevel.Read))
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
