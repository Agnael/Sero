using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public static class Doorman
    {
        public static void HealthCheck(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            var actionDescriptors = actionDescriptorCollectionProvider.ActionDescriptors.Items;
        }
    }
}
