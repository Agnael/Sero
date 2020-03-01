using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class ResourceCodeWithMultipleElementGettersDetected : Exception
    {
        public ResourceCodeWithMultipleElementGettersDetected(string resourceCode) 
            : base("Multiple different MVC actions where found marked with the ElementGetterAttribute decorator, for the '"+ resourceCode + "' resourceCode.")
        {
        }
    }
}
