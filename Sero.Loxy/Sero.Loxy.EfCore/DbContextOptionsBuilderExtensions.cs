using Microsoft.EntityFrameworkCore;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseLoxyAsLoggerFactory(this DbContextOptionsBuilder options, ILoxy loxy, string logCategory)
        {
            options.UseLoggerFactory(
                loxy.AsLoggerFactory(proxy => 
                    proxy.WithCategory(logCategory)
                         .WithStateFormatterFactory(new EfStateFormatterFactory()) // EXPERIMENTAL SHIT, TO BE SURE IT WORKS YOU ARE BETTER OFF USING THE DEFAULT StateFormatter
                )
            );

            return options;
        }
    }
}
