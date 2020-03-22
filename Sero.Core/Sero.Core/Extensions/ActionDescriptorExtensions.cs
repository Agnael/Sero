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

        public static EndpointAttribute GetHateoasAttribute(this ActionDescriptor action)
        {
            var hateoasAttr = (EndpointAttribute)action.EndpointMetadata.FirstOrDefault(x => x is EndpointAttribute);
            return hateoasAttr;
        }

        public static bool IsLinkFor(this ActionDescriptor action, string resourceCode, EndpointScope scope)
        {
            bool isHttpGet = action.IsHttpGet();

            if (!isHttpGet)
                return false;

            var hateoasAttr = action.GetHateoasAttribute();

            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.Scope == scope;
        }

        public static bool IsActionFor(this ActionDescriptor action, string resourceCode, EndpointScope scope)
        {
            bool isHttpGet = action.IsHttpGet();

            if (isHttpGet)
                return false;

            var hateoasAttr = action.GetHateoasAttribute();

            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.Scope == scope;
        }

        public static bool IsOfResource(this ActionDescriptor action, string resourceCode, EndpointScope scope)
        {
            var hateoasAttr = action.GetHateoasAttribute();
            
            return hateoasAttr.ResourceCode == resourceCode
                    && hateoasAttr.Scope == scope;
        }

        public static GetterAttribute GetElementGetterAttribute(this ActionDescriptor action)
        {
            GetterAttribute attr = action.EndpointMetadata.FirstOrDefault(x => x is GetterAttribute) as GetterAttribute;
            return attr;
        }

        public static bool IsElementGetter(this ActionDescriptor action)
        {
            bool isGetter = action.EndpointMetadata.Any(x => x is GetterAttribute);
            return isGetter;
        }

        public static string GetGetterParameterName(this ActionDescriptor action)
        {
            if (!action.IsElementGetter())
                throw new Exception("No tiene sentido obtener el parámetro marcado como GetterParameter en una action NO marcada como Getter.");

            foreach(ControllerParameterDescriptor param in action.Parameters)
            {
                bool isGetterParam = param.ParameterInfo.CustomAttributes.Any(x => x.AttributeType == typeof(GetterParameterAttribute));

                if (isGetterParam)
                    return param.Name;
            }

            return null;
        }
    }
}
