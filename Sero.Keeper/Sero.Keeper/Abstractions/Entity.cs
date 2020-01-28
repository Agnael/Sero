using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Keeper
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
