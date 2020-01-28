using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Keeper.Exceptions
{
    public class UndefinedEntityException : Exception
    {
        public UndefinedEntityException() 
            : base(string.Format("IKeeper: Attempted to use a method that accepts an entity as a parameter, providing a null entity or an entity without an ID."))
        {
        }
    }
}
