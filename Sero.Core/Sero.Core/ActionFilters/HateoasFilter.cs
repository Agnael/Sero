using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sero.Core
{
    public class HateoasFilter : IActionFilter
    {
        public HateoasService HateoasService { get; private set; }
        public Endpoint CurrentEndpoint { get; private set; }
        public string CurrentBaseUrlPath { get; set; }
        public IDictionary<string, object> ProvidedActionArguments { get; private set; }

        public HateoasFilter(HateoasService hateoasService)
        {
            this.HateoasService = hateoasService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {        
            this.ProvidedActionArguments = context.ActionArguments;
            this.CurrentBaseUrlPath = context.HttpContext.Request.Path;

            this.CurrentEndpoint = HateoasService.GetEndpointByAction((ControllerActionDescriptor)context.ActionDescriptor);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult
                && !(context.Result is BadRequestObjectResult))
            {
                var resultValue = (context.Result as ObjectResult).Value;

                if (resultValue is CollectionResult)
                {
                    (context.Result as ObjectResult).Value = GetCollectionView((CollectionResult)resultValue);
                }
                else
                {
                    (context.Result as ObjectResult).Value = GetElementView(resultValue);
                }
            }
        }

        public HateoasElementView GetElementView(object resource)
        {
            string resourceCode = CurrentEndpoint.ResourceCode;
            var elemLinks = GetElementLinks(resourceCode, resource);
            var elemActions = GetHateoasActions(resourceCode, EndpointScope.Element, resource);

            var view = new HateoasElementView(elemLinks, elemActions, resource);
            return view;
        }

        public HateoasCollectionView GetCollectionView(CollectionResult collection)
        {
            // Para que funcione, TODOS los GETs que se hagan de collections, deben tomar SOLO un parámetro, que debe heredar de la clase CollectionFilter
            CollectionFilter filter = collection.UsedFilter;
            string resourceCode = CurrentEndpoint.ResourceCode;
            var collectionLinks = GetCollectionLinks(resourceCode, collection);
            var collectionActions = GetHateoasActions(resourceCode, EndpointScope.Collection);

            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in collection.ElementsToReturn)
            {
                HateoasElementView elementView = GetElementView(element);
                embeddedList.Add(elementView);
            }

            var hateoasCollectionView = new HateoasCollectionView(filter, collection.TotalElementsExisting, collectionLinks, collectionActions, embeddedList);
            return hateoasCollectionView;
        }

        protected string CreateQsPair(string key, string value)
        {
            string result = string.Format("{0}={1}", key.ToLower(), value);
            return result;
        }

        protected string GetCollectionLink(CollectionFilter filter)
        {
            var urlBuilder = new UrlBuilder<CollectionFilter>(this.CurrentBaseUrlPath);

            if (!filter.IsDefaultTextSearch())
                urlBuilder.AddParam(x => x.TextSearch, filter.TextSearch);

            if (!filter.IsDefaultPage())
                urlBuilder.AddParam(x => x.Page, filter.Page);

            if (!filter.IsDefaultPageSize())
                urlBuilder.AddParam(x => x.PageSize, filter.PageSize);

            if (!filter.IsDefaultSortBy())
                urlBuilder.AddParam(x => x.SortBy, filter.SortBy);

            if (!filter.IsDefaultOrderBy())
                urlBuilder.AddParam(x => x.OrderBy, filter.OrderBy);

            string url = urlBuilder.Build();
            return url;
        }

        protected Dictionary<string, string> GetCollectionLinks(string resourceCode, CollectionResult processedResult)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));
            if (processedResult.UsedFilter == null) throw new ArgumentNullException(nameof(processedResult.UsedFilter));
            if (processedResult is null) throw new ArgumentNullException(nameof(processedResult));

            // FILTRA METODOS GET DEL RESOURCECODE ACTUAL
            var usedFilter = processedResult.UsedFilter;
            var relevantLinks = this.HateoasService.GetLinks(resourceCode, EndpointScope.Collection);
            int calculatedLastPage = (int)Math.Ceiling((double)processedResult.TotalElementsExisting / usedFilter.PageSize);

            // POR CADA UNO DE ESOS METODOS, CREA UN LINK. 
            // <NO> CREA LINK SELF
            var linkMap = new Dictionary<string, string>();
            foreach(Endpoint endpoint in relevantLinks)
            {
                if (CurrentEndpoint.EndpointName != endpoint.EndpointName)
                {
                    string href = "/" + endpoint.UrlTemplate;
                    linkMap.Add(endpoint.EndpointName, href);
                }
            }

            // AGREGA SELF DE FORMA CUSTOM PORQUE PUEDE HABER PARAMETROS INNECESARIOS EN EL FILTRO ACTUAL QUE MANDÓ EL USER
            linkMap.Add("self", GetCollectionLink(usedFilter));

            if(CurrentEndpoint.Relation == EndpointRelation.Child)

            // AGREGA LINKS DE PAGINACION
            if (usedFilter.Page >= 1 && usedFilter.Page <= calculatedLastPage)
            {
                if (usedFilter.Page > 1)
                {
                    CollectionFilter prevFilter = usedFilter.Copy();
                    prevFilter.Page = usedFilter.Page > 1 ? usedFilter.Page - 1 : usedFilter.Page; ;
                    string urlPrev = GetCollectionLink(prevFilter);
                    linkMap.Add("prev", urlPrev);

                    CollectionFilter firstFilter = usedFilter.Copy();
                    firstFilter.Page = 1;
                    string urlFirst = GetCollectionLink(firstFilter);
                    linkMap.Add("first", urlFirst);
                }

                if (usedFilter.Page < calculatedLastPage)
                {
                    CollectionFilter nextFilter = usedFilter.Copy();
                    nextFilter.Page = usedFilter.Page < calculatedLastPage ? usedFilter.Page + 1 : usedFilter.Page;
                    string urlNext = GetCollectionLink(nextFilter);
                    linkMap.Add("next", urlNext);

                    CollectionFilter lastFilter = usedFilter.Copy();
                    lastFilter.Page = calculatedLastPage;
                    string urlLast = GetCollectionLink(lastFilter);
                    linkMap.Add("last", urlLast);
                }
            }

            return linkMap;
        }

        protected Dictionary<string, string> GetElementLinks(string resourceCode, object element)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));
            if (element == null) throw new ArgumentNullException(nameof(element));

            var linkMap = new Dictionary<string, string>();
            var relevantEndpoints = HateoasService.GetLinks(resourceCode, EndpointScope.Element);

            foreach (Endpoint endpoint in relevantEndpoints)
            {
                string actionName = endpoint.EndpointName;

                if (endpoint.IsElementGetter)
                    actionName = "self";

                string href = "/" + endpoint.UrlTemplate;

                if (element is IEnumerable
                    && (element as IEnumerable<object>).Count() > 0)
                {
                    ReflectionUtils.ReplaceUrlTemplate(href, (element as IEnumerable<object>).First());
                }
                else
                {
                    ReflectionUtils.ReplaceUrlTemplate(href, element);
                }

                linkMap.Add(actionName, href);
            }
            return linkMap;
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(string resourceCode, EndpointScope scope)
        {
            return this.GetHateoasActions(resourceCode, scope, null);
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(string resourceCode, EndpointScope scope, object valuesSource)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));

            var hateoasActionMap = new Dictionary<string, HateoasAction>();
            var relevantEndpoints = HateoasService.GetActions(resourceCode, scope);

            foreach (Endpoint endpoint in relevantEndpoints)
            {
                HateoasAction newAction = new HateoasAction();
                newAction.Method = endpoint.HttpMethod;
                newAction.Href = "/" + endpoint.UrlTemplate;

                if(valuesSource != null)
                    newAction.Href = ReflectionUtils.ReplaceUrlTemplate(newAction.Href, valuesSource);

                hateoasActionMap.Add(endpoint.EndpointName, newAction);
            }

            return hateoasActionMap;
        }
    }
}
