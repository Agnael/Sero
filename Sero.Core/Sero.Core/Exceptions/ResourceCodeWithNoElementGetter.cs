using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ResourceCodeWithNoElementGetter : Exception
    {
        public ResourceCodeWithNoElementGetter(IEnumerable<string> resourceCodes) 
            : base("No MVC action was found marked with the ElementGetterAttribute decorator, " +
                  "for the resourceCodes: ['"+ string.Join(", ", resourceCodes) + "'].")
        {
        }
    }
}
