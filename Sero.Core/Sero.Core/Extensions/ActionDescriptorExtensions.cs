using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Sero.Core
{
    public static class ActionDescriptorExtensions
    {
        public static string GetHttpMethodValue(this ActionDescriptor action)
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

        public static bool IsHttpGet(this ActionDescriptor action)
        {
            string actionHttpMethod = action.GetHttpMethodValue();
            return actionHttpMethod == HttpMethod.Get.Method;
        }

        public static HttpMethodAttribute GetHttpMethodAttribute(this ActionDescriptor action)
        {
            HttpMethodAttribute attr = 
                action
                .EndpointMetadata
                .OfType<HttpMethodAttribute>()
                .First();

            return attr;
        }

        public static HateoasActionAttribute GetHateoasAttribute(this ActionDescriptor action)
        {
            var hateoasAttr = (HateoasActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is HateoasActionAttribute);
            return hateoasAttr;
        }

        public static bool IsLinkFor(this ActionDescriptor action, string resourceCode, ActionScope scope)
        {
            bool isHttpGet = action.IsHttpGet();

            if (!isHttpGet)
                return false;

            var hateoasAttr = action.GetHateoasAttribute();

            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.ActionScope == scope;
        }

        public static bool IsActionFor(this ActionDescriptor action, string resourceCode, ActionScope scope)
        {
            bool isHttpGet = action.IsHttpGet();

            if (isHttpGet)
                return false;

            var hateoasAttr = action.GetHateoasAttribute();

            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.ActionScope == scope;
        }

        public static bool IsOfResource(this ActionDescriptor action, string resourceCode, ActionScope scope)
        {
            var hateoasAttr = action.GetHateoasAttribute();
            
            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.ActionScope == scope;
        }

        public static bool IsElementGetter(this ActionDescriptor action)
        {
            bool isGetter = action.EndpointMetadata.Any(x => x is ElementGetterAttribute);
            return isGetter;
        }
    }
}
