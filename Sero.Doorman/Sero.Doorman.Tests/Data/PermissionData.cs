using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests
{
    public static class PermissionData
    {
        public static Permission Resource_01_Read => new Permission(ResourceData.Resource_01.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_01_ReadWrite => new Permission(ResourceData.Resource_01.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_02_Read => new Permission(ResourceData.Resource_02.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_02_ReadWrite => new Permission(ResourceData.Resource_02.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_03_Read => new Permission(ResourceData.Resource_03.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_03_ReadWrite => new Permission(ResourceData.Resource_03.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_04_Read => new Permission(ResourceData.Resource_04.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_04_ReadWrite => new Permission(ResourceData.Resource_04.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_05_Read => new Permission(ResourceData.Resource_05.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_05_ReadWrite => new Permission(ResourceData.Resource_05.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_06_Read => new Permission(ResourceData.Resource_06.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_06_ReadWrite => new Permission(ResourceData.Resource_06.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_07_Read => new Permission(ResourceData.Resource_07.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_07_ReadWrite => new Permission(ResourceData.Resource_07.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_08_Read => new Permission(ResourceData.Resource_08.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_08_ReadWrite => new Permission(ResourceData.Resource_08.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_09_Read => new Permission(ResourceData.Resource_09.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_09_ReadWrite => new Permission(ResourceData.Resource_09.Code, PermissionLevel.ReadWrite);

        public static Permission Resource_10_Read => new Permission(ResourceData.Resource_10.Code, PermissionLevel.ReadOnly);
        public static Permission Resource_10_ReadWrite => new Permission(ResourceData.Resource_10.Code, PermissionLevel.ReadWrite);
    }
}
