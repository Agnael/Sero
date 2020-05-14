using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IPage<out T>
    {
        int Total { get; }
        IEnumerable<T> Items { get; }
        public bool IsEmpty { get; }
    }
}
