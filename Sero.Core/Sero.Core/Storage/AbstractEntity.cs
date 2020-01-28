using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Storage
{
    public abstract class AbstractEntity : IEquatable<AbstractEntity>
    {
        public int Id { get; set; }

        public bool Equals(AbstractEntity other)
        {
            var isEqual = this.Id.Equals(other.Id);
            return isEqual;
        }
    }
}
