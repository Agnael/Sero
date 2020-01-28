using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Keeper.Exceptions
{
    public class DeleteUnexistingEntityException : Exception
    {
        public DeleteUnexistingEntityException(int id) 
            : base(string.Format("IKeeper: Attempted to delete an unexisting entity (ID: {0}).", id))
        {
        }
    }
}
