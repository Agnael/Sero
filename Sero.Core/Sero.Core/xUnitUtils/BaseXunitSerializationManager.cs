using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Core
{
    public interface IXunitSerializationManager<TObj>
    {
        void Serialize(IXunitSerializationInfo info, TObj objInstanceRef);
        void Deserialize(IXunitSerializationInfo info, TObj objInstanceRef);
    }
}
