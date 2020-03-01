using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class Endpoint
    {
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

        /// <summary>
        ///     Indica si este endpoint es un link (GET) o una acción (POST, PUT, DELETE) sobre este resourceCode.
        /// </summary>
        public readonly EndpointType Type;

        public readonly EndpointScope Scope;

        public readonly EndpointRelation Relation;

        public readonly string UrlTemplate;

        public readonly string HttpMethod;

        public Endpoint(
            string resourceCode,
            string mvcActionName,
            string endpointName,
            bool isElementGetter,
            EndpointType type,
            EndpointScope scope,
            EndpointRelation relation,
            string urlTemplate,
            string httpMethod)
        {
            this.ResourceCode = resourceCode;
            this.MvcActionName = mvcActionName;
            this.EndpointName = endpointName;
            this.IsElementGetter = isElementGetter;
            this.Type = type;
            this.Scope = scope;
            this.Relation = relation;
            this.UrlTemplate = urlTemplate;
            this.HttpMethod = httpMethod;
        }
    }
}