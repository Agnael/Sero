using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public static class PermissionData
    {
        public static Permission Resource_01_R_R => new Permission(ResourceData.Resource_01.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_01_RW_R => new Permission(ResourceData.Resource_01.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_02_R_R => new Permission(ResourceData.Resource_02.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_02_RW_R => new Permission(ResourceData.Resource_02.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_03_R_R => new Permission(ResourceData.Resource_03.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_03_RW_R => new Permission(ResourceData.Resource_03.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_04_R_R => new Permission(ResourceData.Resource_04.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_04_RW_R => new Permission(ResourceData.Resource_04.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_05_R_R => new Permission(ResourceData.Resource_05.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_05_RW_R => new Permission(ResourceData.Resource_05.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_06_R_R => new Permission(ResourceData.Resource_06.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_06_RW_R => new Permission(ResourceData.Resource_06.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_07_R_R => new Permission(ResourceData.Resource_07.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_07_RW_R => new Permission(ResourceData.Resource_07.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_08_R_R => new Permission(ResourceData.Resource_08.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_08_RW_R => new Permission(ResourceData.Resource_08.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_09_R_R => new Permission(ResourceData.Resource_09.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_09_RW_R => new Permission(ResourceData.Resource_09.Code, PermissionLevel.Write, PermissionLevel.Read);

        public static Permission Resource_10_R_R => new Permission(ResourceData.Resource_10.Code, PermissionLevel.Read, PermissionLevel.Read);
        public static Permission Resource_10_RW_R => new Permission(ResourceData.Resource_10.Code, PermissionLevel.Write, PermissionLevel.Read);
    }
}
