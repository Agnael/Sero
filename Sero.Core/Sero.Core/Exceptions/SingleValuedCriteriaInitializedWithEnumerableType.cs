using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class SingleValuedCriteriaInitializedWithEnumerableType : Exception
    {
        public SingleValuedCriteriaInitializedWithEnumerableType() 
            : base("Can't create a SingleValuedCriteria by passing an IEnumerable type.")
        {
        }
    }
}
