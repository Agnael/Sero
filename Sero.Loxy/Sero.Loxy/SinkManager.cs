using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class SinkManager
    {
        public readonly LoxyBuilder CurrentLoxyBuilder;

        public IList<ISink> Sinks { get; private set; }

        public SinkManager(LoxyBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            CurrentLoxyBuilder = builder;
            Sinks = new List<ISink>();
        }

        public LoxyBuilder AddSink(ISink sink)
        {
            if (sink == null) throw new ArgumentNullException(nameof(sink));

            Sinks.Add(sink);
            return CurrentLoxyBuilder;
        }
    }
}
