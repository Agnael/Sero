using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public static class ResourceData
    {
        public const string RESOURCE_01_CODE = "resource_code_01";
        public const string RESOURCE_02_CODE = "resource_code_02";
        public const string RESOURCE_03_CODE = "resource_code_03";
        public const string RESOURCE_04_CODE = "resource_code_04";
        public const string RESOURCE_05_CODE = "resource_code_05";
        public const string RESOURCE_06_CODE = "resource_code_06";
        public const string RESOURCE_07_CODE = "resource_code_07";
        public const string RESOURCE_08_CODE = "resource_code_08";
        public const string RESOURCE_09_CODE = "resource_code_09";
        public const string RESOURCE_10_CODE = "resource_code_10";

        public const string RESOURCE_CATEGORY_01 = "Category1";
        public const string RESOURCE_CATEGORY_02 = "Category2";
        public const string RESOURCE_CATEGORY_03 = "Category3";

        public static Resource Resource_01 => new Resource(RESOURCE_CATEGORY_01, RESOURCE_01_CODE, "Resource 1 description");
        public static Resource Resource_02 => new Resource(RESOURCE_CATEGORY_01, RESOURCE_02_CODE, "Resource 2 description");
        public static Resource Resource_03 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_03_CODE, "Resource 3 description");
        public static Resource Resource_04 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_04_CODE, "Resource 4 description");
        public static Resource Resource_05 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_05_CODE, "Resource 5 description");
        public static Resource Resource_06 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_06_CODE, "Resource 6 description");
        public static Resource Resource_07 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_07_CODE, "Resource 7 description");
        public static Resource Resource_08 => new Resource(RESOURCE_CATEGORY_02, RESOURCE_08_CODE, "Resource 8 description");
        public static Resource Resource_09 => new Resource(RESOURCE_CATEGORY_03, RESOURCE_09_CODE, "Resource 9 description");
        public static Resource Resource_10 => new Resource(RESOURCE_CATEGORY_03, RESOURCE_10_CODE, "Resource 10 description");
    }
}
