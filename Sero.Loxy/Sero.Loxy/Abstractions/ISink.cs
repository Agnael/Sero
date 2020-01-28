using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Loxy.Abstractions
{
    public interface ISink
    {
        Task PersistAsync(IRequestInfo requestInfo);

        /// <summary>
        /// Minimum LogLevel a request must have to be processed by this Sink
        /// </summary>
        LogLevel MinimumLevel { get; }

        /// <summary>
        /// If the request has AT LEAST ONE event with this LogLevel, then ALL the events of the request are
        /// processed by the Sink, ignoring the MinimumLevel requirement.
        /// </summary>
        LogLevel ExtendedLevel { get; }
    }
}
