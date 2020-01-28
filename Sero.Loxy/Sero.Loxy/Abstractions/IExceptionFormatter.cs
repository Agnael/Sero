using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IExceptionFormatter
    {
        IEnumerable<ExceptionInfo> Format(Exception ex);
    }
}
