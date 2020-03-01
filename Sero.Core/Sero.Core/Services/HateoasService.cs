using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Sero.Core
{
    public class HateoasService
    {
        private readonly IEnumerable<Endpoint> _endpoints;

        public HateoasService(IActionDescriptorCollectionProvider actionCollectionProvider)
        {
            if (actionCollectionProvider == null 
                || actionCollectionProvider.ActionDescriptors == null 
                || actionCollectionProvider.ActionDescriptors.Items == null) 
                throw new ActionDescriptorCollectionProviderServiceNotFound();

            var actions = actionCollectionProvider.ActionDescriptors.Items;

            HealthCheck(actions);

            // Prepara la metadata que se va a usar durante las responses para generar las navegaciones correspondiente
            var endpoints = new List<Endpoint>();
            foreach (ControllerActionDescriptor action in actions)
            {
                var hateoasAttr = action.GetHateoasAttribute();
                bool isElementGetter = action.IsElementGetter();

                string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);
                EndpointType type = EndpointType.Action;
                HttpMethodAttribute httpMethodAttr = action.GetHttpMethodAttribute();
                string httpMethod = action.GetHttpMethodValue();

                if (httpMethod == HttpMethods.Get)
                    type = EndpointType.Link;

                Endpoint newEndpoint = new Endpoint(
                    hateoasAttr.ResourceCode,
                    action.ActionName,
                    actionName,
                    isElementGetter,
                    type,
                    hateoasAttr.Scope,
                    hateoasAttr.Relation,
                    httpMethodAttr.Template,
                    httpMethod);

                endpoints.Add(newEndpoint);
            }

            _endpoints = endpoints;
        }

        public Endpoint GetEndpointByAction(ControllerActionDescriptor action)
        {
            var hateoasAttr = action.GetHateoasAttribute();

            Endpoint found = _endpoints
                .FirstOrDefault(x => 
                    x.ResourceCode == hateoasAttr.ResourceCode
                    && x.Scope == hateoasAttr.Scope
                    && x.MvcActionName == action.ActionName);

            return found;
        }

        public IEnumerable<Endpoint> GetLinks(string resourceCode, EndpointScope scope)
        {
            var links =
                _endpoints
                .Where(x => x.Type == EndpointType.Link
                    && x.Scope == scope
                    && x.ResourceCode == resourceCode);

            return links;
        }

        public IEnumerable<Endpoint> GetActions(string resourceCode, EndpointScope scope)
        {
            var actions =
                _endpoints
                .Where(x => x.Type == EndpointType.Action
                    && x.Scope == scope
                    && x.ResourceCode == resourceCode);

            return actions;
        }

        public string GetElementGetterUrl(Element resource)
        {
            if (resource == null) throw new ArgumentNullException();

            string resourceCode = resource.GetAppResourceCode();

            Endpoint getterEndpoint = 
                _endpoints
                .FirstOrDefault(x => x.IsElementGetter
                    && x.ResourceCode == resourceCode);

            string url = ReflectionUtils.ReplaceUrlTemplate("/" + getterEndpoint.UrlTemplate, resource);
            return url;
        }

        private void HealthCheck(IReadOnlyList<ActionDescriptor> actions)
        {
            // Diccionario de <resourceCode, hasDefinedElementGetter>
            var resourceMap = new Dictionary<string, bool>();

            foreach (ControllerActionDescriptor action in actions)
            {
                var hateoasAttr = action.GetHateoasAttribute();

                if (hateoasAttr == null)
                    throw new MvcActionWithNoEndpointAttribute(action);

                string resourceCode = hateoasAttr.ResourceCode;

                if (!resourceMap.ContainsKey(resourceCode))
                    resourceMap.Add(resourceCode, false);

                // Chequea que el resourceCode tenga una MVC action marcada como ElementGetter
                bool isElementGetter = action.IsElementGetter();
                if (isElementGetter)
                {
                    if (hateoasAttr.Scope != EndpointScope.Element)
                        throw new ElementGetterAttributeInANonElementScopedEndpoint(action);

                    if (hateoasAttr.Relation != EndpointRelation.Parent)
                        throw new ElementGetterAttributeInANonParentEndpoint(action);

                    // No puede haber múltiples acciones marcadas como ElementGetter para el mismo resourceCode

                    // TODO: Comento este chequeo porque al darle varias rutas posibles a la misma acción, el framework los toma como rutas completamente distintas, con una copia
                    // del ElementGetterAttribute cada una, por más que sea el mismo bloque de codigo
                    //if (resourceMap[resourceCode] == true)
                    //    throw new ResourceCodeWithMultipleElementGettersDetected(resourceCode);

                    resourceMap[resourceCode] = true;
                }
            }

            // Revisa si algun resourceCode no tiene acción marcada con ElementGetter
            IEnumerable<string> resourceCodesWithNoElementGetter =
                resourceMap
                .Where(x => x.Value == false)
                .Select(x => x.Key);

            if (resourceCodesWithNoElementGetter != null
                && resourceCodesWithNoElementGetter.Count() > 0)
                throw new ResourceCodeWithNoElementGetter(resourceCodesWithNoElementGetter);
        }
    }
}
