using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ElementGetterAttributeInANonElementScopedEndpoint : Exception
    {
        public ElementGetterAttributeInANonElementScopedEndpoint(ControllerActionDescriptor action) 
            : base("An MVC action ('"+action.DisplayName+"') can't have the ElementGetterAttribute because it is marked as an endpoint with a Collection scope.")
        {
        }
    }
}
