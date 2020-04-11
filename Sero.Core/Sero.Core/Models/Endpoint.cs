using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class Endpoint
    {
        public readonly ControllerActionDescriptor Action;

        public readonly string DisplayNameWhenLinked;

        public readonly string ResourceCode;

        /// <summary>
        ///     Nombre del endpoint, en snake case. Sirve para que el usuario reciba un nombre descriptivo sobre cada link/action 
        ///     que recibe.
        /// </summary>
        public readonly string EndpointName;

        public readonly string MvcActionName;

        /// <summary>
        ///     Indica si este es el endpoint en el que, a partir de su template, se puede obtener un elemento concreto
        ///     del recurso que corresponde al ResourceCode del getter.
        ///     
        ///     Solo puede haber un ElementGetter por recurso.
        /// </summary>
        public readonly bool IsElementGetter;
        public readonly string GetterParameterName;

        /// <summary>
        ///     Indica si este endpoint es un link (GET) o una acción (POST, PUT, DELETE) sobre este resourceCode.
        /// </summary>
        public readonly EndpointType Type;

        public readonly EndpointScope Scope;

        public readonly string UrlTemplate;

        public readonly string HttpMethod;

        public Endpoint(ControllerActionDescriptor action)
        {
            var elementGetterAttr = action.GetElementGetterAttribute();
            bool isElementGetter = false;
            string getterParameterName = null;
            string getterDisplayNameWhenLinked = null;

            if (elementGetterAttr != null)
            {
                isElementGetter = true;
                getterDisplayNameWhenLinked = elementGetterAttr.DisplayNameWhenLinked;
                getterParameterName = action.GetGetterParameterName();

                if (string.IsNullOrEmpty(getterParameterName))
                    throw new Exception("Si el endpoint es un ElementGetter, entonces es obligatorio proporcionar un GetterParameterName NO NULO.");
            }

            string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);
            EndpointType type = EndpointType.Action;
            HttpMethodAttribute httpMethodAttr = action.GetHttpMethodAttribute();
            string httpMethod = action.GetHttpMethodValue();

            if (httpMethod == HttpMethods.Get)
                type = EndpointType.Link;

            var hateoasAttr = action.GetHateoasAttribute();

            this.Action = action;

            this.IsElementGetter = isElementGetter;
            this.GetterParameterName = getterParameterName;

            this.DisplayNameWhenLinked = getterDisplayNameWhenLinked;
            this.ResourceCode = hateoasAttr.ResourceCode;
            this.MvcActionName = action.ActionName;
            this.EndpointName = actionName;
            this.Type = type;
            this.Scope = hateoasAttr.Scope;
            this.UrlTemplate = httpMethodAttr.Template;
            this.HttpMethod = httpMethod;
        }
    }
}