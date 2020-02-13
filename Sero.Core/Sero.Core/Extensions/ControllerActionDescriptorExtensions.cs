using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core
{
    public static class ControllerActionDescriptorExtensions
    {
        public static string GetHttpMethodValue(this ControllerActionDescriptor action)
        {
            string actionHttpMethod =
                action
                .EndpointMetadata
                .OfType<HttpMethodAttribute>()
                .SelectMany(x => x.HttpMethods)
                .Distinct()
                .First();

            return actionHttpMethod;
        }

        public static HttpMethodAttribute GetHttpMethodAttribute(this ControllerActionDescriptor action)
        {
            HttpMethodAttribute attr = 
                action
                .EndpointMetadata
                .OfType<HttpMethodAttribute>()
                .First();

            return attr;
        }

        public static HateoasActionAttribute GetHateoasAttribute(this ControllerActionDescriptor action)
        {
            var hateoasAttr = (HateoasActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is HateoasActionAttribute);
            return hateoasAttr;
        }

        public static bool IsElementGetter(this ControllerActionDescriptor action)
        {
            bool isGetter = action.EndpointMetadata.Any(x => x is ElementGetterAttribute);
            return isGetter;
        }
    }
}
