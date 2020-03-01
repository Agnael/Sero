using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class MvcActionWithNoEndpointAttribute : Exception
    {
        public MvcActionWithNoEndpointAttribute(ActionDescriptor action) 
            : base("The MVC action ('"+action.DisplayName+"') was found with no EndpointAttribute decorator set. " +
                  "This attribute is necessary for Sero.Core to map it's HATEOAS navigations.")
        {
        }
    }
}
