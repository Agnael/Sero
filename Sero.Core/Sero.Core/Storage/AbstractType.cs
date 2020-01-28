using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Storage
{
    public abstract class AbstractType : AbstractEntity, IEquatable<AbstractType>
    {
        public string Description { get; set; }

        public bool Equals(AbstractType other)
        {
            var isEqual = base.Equals(other)
                           && this.Description.Equals(other.Description);
            return isEqual;
        }
    }
}
