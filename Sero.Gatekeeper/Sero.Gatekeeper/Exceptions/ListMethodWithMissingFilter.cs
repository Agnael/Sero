using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class ListMethodWithMissingFilter : Exception
    {
        public ListMethodWithMissingFilter() 
            : base("Gatekeeper error: Even if the user didn't explicitly specify any search parameters, the used filter MUST have default values asigned for the" +
                  "basic CollectionFilter class properties for gatekeeper to generate HATEOAS context for the returning resource list.")
        {
        }
    }
}
