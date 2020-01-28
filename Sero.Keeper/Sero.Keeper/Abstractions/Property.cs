using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Keeper
{
    public abstract class Property : Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
