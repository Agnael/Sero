using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IApiResource
    {
        string OwnerId { get; }
        string ApiResourceCode { get; }
    }
}