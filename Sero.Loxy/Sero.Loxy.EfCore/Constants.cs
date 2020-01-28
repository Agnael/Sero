using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    internal class Constants
    {
        public static readonly KeyValuePair<String, Object> Null_Kvp = default(KeyValuePair<String, Object>);

        // Keys that can be found when reading a log's state as a dictionary
        public const string Key_MessageTemplate = "{OriginalFormat}";
        public const string Key_NewLine = "newLine";
        public const string Key_CommandText = "commandText";
    }
}
