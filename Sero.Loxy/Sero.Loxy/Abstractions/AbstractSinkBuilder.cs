using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public abstract class AbstractSinkBuilder<T>
    {
        /// <summary>
        ///     Minimum LogLevel a request must have to be processed by this Sink.
        /// </summary>
        public LogLevel LevelMinimum { get; private set; }

        /// <summary>
        ///     If the request has AT LEAST ONE event with this LogLevel, then ALL the events of the request are
        ///     processed by the Sink, ignoring the MinimumLevel requirement.
        /// </summary>
        public LogLevel LevelExtended { get; private set; }

        public abstract T CurrentInstance { get; }
        public readonly SinkManager Sinks;

        public AbstractSinkBuilder(SinkManager currentSinkManager)
        {
            Sinks = currentSinkManager;

            LevelMinimum = LogLevel.Information;
            LevelExtended = LogLevel.Error;
        }

        /// <summary>
        /// Minimum LogLevel a request must have to be processed by this Sink
        /// </summary>
        public T WithMinimumLevel(LogLevel level)
        {
            LevelMinimum = level;
            return CurrentInstance;
        }

        /// <summary>
        /// If the request has AT LEAST ONE event with this LogLevel, then ALL the events of the request are
        /// processed by the Sink, ignoring the MinimumLevel requirement.
        /// </summary>
        public T WithExtendedLevel(LogLevel level)
        {
            LevelExtended = level;
            return CurrentInstance;
        }
    }
}
