using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public interface IResultView
    {
        string ResourceCode { get; }
    }
}
