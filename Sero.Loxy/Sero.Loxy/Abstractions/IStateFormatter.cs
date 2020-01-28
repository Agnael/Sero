using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IStateFormatter<TState>
    {
        IEnumerable<string> Format(TState state);
    }
}
