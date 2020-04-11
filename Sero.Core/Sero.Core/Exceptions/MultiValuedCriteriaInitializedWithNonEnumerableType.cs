using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class MultiValuedCriteriaInitializedWithNonEnumerableType : Exception
    {
        public MultiValuedCriteriaInitializedWithNonEnumerableType() 
            : base("Can't create a MultiValuedCriteria by passing a non IEnumerable type.")
        {
        }
    }
}
