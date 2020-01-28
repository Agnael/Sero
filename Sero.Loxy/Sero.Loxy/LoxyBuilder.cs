using Microsoft.Extensions.DependencyInjection;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoxyBuilder
    {
        public readonly IServiceCollection ServiceCollection;

        public readonly SinkManager Sinks;

        public LoxyBuilder(IServiceCollection services)
        {
            ServiceCollection = services;
            Sinks = new SinkManager(this);
        }
    }
}
