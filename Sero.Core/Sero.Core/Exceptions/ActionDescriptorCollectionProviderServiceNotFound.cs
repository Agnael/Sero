using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ActionDescriptorCollectionProviderServiceNotFound : Exception
    {
        public ActionDescriptorCollectionProviderServiceNotFound() 
            : base("Could not access the IActionDescriptorCollectionProvider.ActionDescriptors.Items property. Please" +
                  "check that the Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider service is correctly registered " +
                  "(it should be automatically, since it's an ASP.NET Core MVC injected service) and all of the listed properties are not null.")
        {
        }
    }
}
