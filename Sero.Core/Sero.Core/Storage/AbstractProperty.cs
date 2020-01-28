using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Storage
{
    public abstract class AbstractProperty : AbstractEntity, IEquatable<AbstractProperty>
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool Equals(AbstractProperty other)
        {
            var isEqual = base.Equals(other)
                            && this.Key.Equals(other.Key)
                            && this.Value.Equals(other.Value);
            return isEqual;
        }
    }
}
