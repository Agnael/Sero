using Newtonsoft.Json;
using Sero.Core;
using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Doorman
{
    public class Permission : IXunitSerializable
    {
        public string ResourceCode { get; set; }
        public PermissionLevel Level { get; set; }

        public Permission()
        {

        }

        public Permission(string resourceCode, PermissionLevel level)
        {
            this.ResourceCode = resourceCode;
            this.Level = level;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            this.ResourceCode = info.GetValue<string>(nameof(ResourceCode));
            this.Level = info.GetValue<PermissionLevel>(nameof(Level));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(ResourceCode), this.ResourceCode);
            info.AddValue(nameof(Level), this.Level);
        }
    }
}
