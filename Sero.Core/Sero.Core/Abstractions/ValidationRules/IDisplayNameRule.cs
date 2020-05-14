using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IDisplayNameRule : IBaseRule<string>
    {
        int LengthMin { get; }
        int LengthMax { get;}
        string RegexPattern { get;}
    }
}