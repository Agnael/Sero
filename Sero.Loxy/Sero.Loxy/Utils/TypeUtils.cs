using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy.Utils
{
    public static class TypeUtils
    {
        public static string GetFriendlyName(Type type)
        {
            if (type.IsGenericType)
            {
                return string.Format("{0}<{1}>", 
                                    type.Name.Split('`')[0], 
                                    string.Join(", ", type.GetGenericArguments().Select(x => GetFriendlyName(x))));
            }
            else
                return type.Name;
        }
    }
}
