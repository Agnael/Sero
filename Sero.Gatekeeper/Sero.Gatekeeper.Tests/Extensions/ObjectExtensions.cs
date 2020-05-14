using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public static class ObjectExtensions
    {
        public static object[] ToArray(this object obj)
        {
            return new object[] { obj };
        }
    }
}
