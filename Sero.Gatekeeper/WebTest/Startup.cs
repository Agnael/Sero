using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sero.Gatekeeper;
using Serilog;
using Sero.Loxy;
using Newtonsoft.Json;
using Serilog.Filters;
using Sero.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using Sero.Gatekeeper.Storage;
using Sero.Sentinel.Storage;
using Sero.Sentinel;

namespace WebTest
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSerilog();

            services.AddLogging(x => x.AddSerilog());

            services.TryAddSingleton<HateoasService>();

            services.AddRequestInfo();
            services.AddLoxy()
                .Sinks.AddJsonSink()
                    .WithMinimumLevel(LogLevel.Debug)
                    .WithExtendedLevel(LogLevel.Error)
                    .WithJsonFormatting(Formatting.Indented);

            // Configurations
            services.AddOptions<GtkOptions>()
                .Bind(Config.GetSection("Gatekeeper"))
                .ValidateEagerly();
            
            // Stores
            services.AddSingleton<IResourceStore>(_ => new InMemoryResourceStore(GetTestResources()));
            services.AddSingleton<IRoleStore>(_ => new InMemoryRoleStore(GetTestRoles()));
            services
                .AddSingleton<ICredentialStore>(serviceCollection => 
                {
                    ISessionStore sessionStore = serviceCollection.GetService<ISessionStore>();
                    return new InMemoryCredentialStore(sessionStore, GetTestCredentials()); 
                });
            services.AddSingleton<ISessionStore>(_ => new InMemorySessionStore());
            services.AddSingleton<ILoginAttemptStore>(_ => new InMemoryLoginAttemptStore());
            services.AddSingleton<ICredentialPenaltyStore>(_ => new InMemoryCredentialPenaltyStore());

            services.AddTransient<ILoginAttemptLimitingService, DefaultLoginAttemptLimitingService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            ILoxy loxy = context.HttpContext.RequestServices.GetService<ILoxy>();
                            loxy.Raise(new Event(LogLevel.Information, "Gatekeeper", context.Principal.GetCredentialId()));
                        }
                    };

                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        //ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Config["Auth:Issuer"],
                        ValidAudience = Config["Auth:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Auth:SigningSecret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddGatekeeper();

            services.AddControllers(conf => {
                conf.Filters.Add<GtkActionFilter>();
                conf.Filters.Add<HateoasFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                IdentityModelEventSource.Logger.LogLevel = System.Diagnostics.Tracing.EventLevel.LogAlways;
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseLoxy();
            app.UseGatekeeper();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            //app.UseAuthorization();

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

        public List<Credential> GetTestCredentials()
        {
            var testRoles = GetTestRoles();

            var credentials = new List<Credential>();

            string newId = "";
            
            Credential cred1 = new Credential();
            cred1.CredentialId = "agnael";
            cred1.DisplayName = "Agnael";
            cred1.CreationDate = new DateTime(2015, 5, 4);
            cred1.Email = "oleg.kuzmych@gmail.com";
            cred1.PasswordSalt = "ABCDEFGHIJKLMNOP";
            cred1.PasswordHash = HashingUtil.GenerateHash("12345678", cred1.PasswordSalt);
            cred1.BirthDate = new DateTime(1994, 10, 26);

            // TODO: CAMBIE EL SISTEMA DE ROLES PARA QUE HAYA POR CREDENCIAL Y POR USUARIO DE CADA API, ASIQUE TODO ESTO YA NO TIENE SENTIDO Y HAY QUE CAMBIARLo

            //cred1.Roles = new List<Role>
            //{
            //    testRoles[0],
            //    testRoles[1],
            //    testRoles[2],
            //    testRoles[3]
            //};

            Credential cred2 = new Credential();
            cred2.CredentialId = "simbad";
            cred2.DisplayName = "Simbad";
            cred2.CreationDate = new DateTime(2015, 5, 4);
            cred2.Email = "random.pibito@gmail.com";
            cred2.PasswordSalt = "ABCDEFGHIJKLMNOP";
            cred2.PasswordHash = HashingUtil.GenerateHash("12345678", cred1.PasswordSalt);
            cred2.BirthDate = new DateTime(1998, 6, 2);
            //cred2.Roles = new List<Role>
            //{
            //    testRoles[1],
            //    testRoles[3]
            //};

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
                    "Gatekeeper administrator",
                    "Has access to any gatekeeper endpoint. Can manage any security aspect of the application.",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete),
                    new Permission("dm_res_roles", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin02",
                    "TEST02",
                    "DESC02",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin03",
                    "TEST03",
                    "DESC03",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin04",
                    "TEST04",
                    "DESC04",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin05",
                    "TEST05",
                    "DESC05",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin06",
                    "TEST06",
                    "DESC06",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin07",
                    "TEST07",
                    "DESC07",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin08",
                    "TEST08",
                    "DESC08",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin09",
                    "TEST09",
                    "DESC09",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Write)),
                new Role(
                    "dm_role_admin10",
                    "TEST10",
                    "DESC10",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete)),
                new Role(
                    "dm_role_admin11",
                    "TEST11",
                    "DESC11",
                    new Permission("dm_res_resources", PermissionLevel.Complete, PermissionLevel.Complete))
            };
        }

        public List<Resource> GetTestResources()
        {
            return new List<Resource>
            {

            };
        }
    }
}
