using Newtonsoft.Json;
using Sero.Core;
using Sero.Gatekeeper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Gatekeeper.Storage
{
    public class Permission : IXunitSerializable
    {
        public string ResourceCode { get; set; }
        public PermissionLevel LevelOnOwned { get; set; }
        public PermissionLevel LevelOnAny { get; set; }

        public Permission()
        {

        }

        public Permission(string resourceCode, PermissionLevel levelOnOwned, PermissionLevel levelOnAny)
        {
            this.ResourceCode = resourceCode;
            this.LevelOnOwned = levelOnOwned;
            this.LevelOnAny = LevelOnAny;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            this.ResourceCode = info.GetValue<string>(nameof(ResourceCode));
            this.LevelOnOwned = info.GetValue<PermissionLevel>(nameof(LevelOnOwned));
            this.LevelOnAny = info.GetValue<PermissionLevel>(nameof(LevelOnAny));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(ResourceCode), this.ResourceCode);
            info.AddValue(nameof(LevelOnOwned), this.LevelOnOwned);
            info.AddValue(nameof(LevelOnAny), this.LevelOnAny);
        }
    }
}
