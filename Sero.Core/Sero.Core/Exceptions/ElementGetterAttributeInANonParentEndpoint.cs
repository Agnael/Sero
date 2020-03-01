using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ElementGetterAttributeInANonParentEndpoint : Exception
    {
        public ElementGetterAttributeInANonParentEndpoint(ControllerActionDescriptor action)
            : base("An MVC action ('" + action.DisplayName + "') can't have the ElementGetterAttribute because it is marked as an endpoint as a Parent (as the EndpointRelation value).")
        {
        }
    }
}
