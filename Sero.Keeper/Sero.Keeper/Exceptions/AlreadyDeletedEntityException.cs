using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Keeper.Exceptions
{
    public class AlreadyDeletedEntityException : Exception
    {
        public AlreadyDeletedEntityException(int id) 
            : base(string.Format("IKeeper: Attempted to delete an existing entity (ID: {0}) which is already marked as deleted.", id))
        {
        }
    }
}
