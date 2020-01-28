using Microsoft.EntityFrameworkCore;
using Sero.Keeper.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Sero.Loxy.Abstractions;
using Sero.Loxy;
using Sero.Loxy.EfCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebTest
{
    public class AppKeeper : EfKeeper
    {
        private readonly ILoxy _loxy;

        public AppKeeper(ILoxy loxy)
        {
            if (loxy == null) throw new ArgumentNullException(nameof(loxy));

            _loxy = loxy;
        }

        protected override DbContext CreateDbContext()
        {
            var optionBuilder = new DbContextOptionsBuilder()
                                    .EnableSensitiveDataLogging()
                                    .ConfigureWarnings(opts => {

                                        // Evita que se tire un log de tipo warning solo para decir que se loggean los valores de los parametros de cada query. Sin los
                                        // valores cual es el punto de loggear esta verga?
                                        opts.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning);

                                        // Sarasa de modo debug demasiado específico de EFCore como para ser util para propositos generales y es demasiado texo
                                        // Ejemplo: https://pastebin.com/q8wb7YsT
                                        opts.Ignore(CoreEventId.QueryExecutionPlanned);
                                    })
                                    .UseLoxyAsLoggerFactory(_loxy, "EFCore")
                                    .UseNpgsql("Host=localhost;Database=keeper_test;Username=postgres;Password=Mogumbo1-");

            AppDbContext context = new AppDbContext(optionBuilder.Options);
            return context;
        }
    }
}
