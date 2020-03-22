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
        public ActionDescriptor CurrentActionDescriptor { get; private set; }
        public string CurrentBaseUrlPath { get; set; }
        public IDictionary<string, object> ProvidedActionArguments { get; private set; }

        public HateoasFilter(HateoasService hateoasService)
        {
            this.HateoasService = hateoasService;
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {        
            this.ProvidedActionArguments = context.ActionArguments;
            this.CurrentBaseUrlPath = context.HttpContext.Request.Path;

            this.CurrentEndpoint = HateoasService.GetEndpointByAction((ControllerActionDescriptor)context.ActionDescriptor);
            this.CurrentActionDescriptor = context.ActionDescriptor;
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult
                && !(context.Result is BadRequestObjectResult))
            {
                var resultValue = (context.Result as ObjectResult).Value;

                if (resultValue is CollectionView)
                {
                    CollectionView output = (CollectionView)resultValue;
                    (context.Result as ObjectResult).Value = GetHateoasView(output);
                    return;
                }
                else
                {
                    ElementView output = (ElementView)resultValue;
                    (context.Result as ObjectResult).Value = GetHateoasView(output.ResourceCode, output.ViewModel);
                    return;
                }
            }
        }

        private HateoasLabeledLink GetParentLink(string parentResourceCode, string childResourceCode)
        {
            HateoasLabeledLink parentLink = null;

            if (childResourceCode != parentResourceCode)
            {
                Endpoint parentGetter = HateoasService.GetElementGetter(parentResourceCode);
                object getterParamValue = ProvidedActionArguments[parentGetter.GetterParameterName];

                string parentUrl = HateoasService.GetElementGetterUrl(parentResourceCode, parentGetter.GetterParameterName, getterParamValue);
                parentLink = new HateoasLabeledLink(parentGetter.DisplayNameWhenLinked, parentUrl);
            }

            return parentLink;
        }

        private HateoasCollectionView GetHateoasView(CollectionView output)
        {
            string parentResourceCode = CurrentEndpoint.ResourceCode;
            string currentResourceCode = output.ResourceCode;

            var collectionLinks = GetCollectionLinks(currentResourceCode, output);
            var collectionActions = GetHateoasActions(currentResourceCode, EndpointScope.Collection);
            
            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in output.ViewModels)
            {
                HateoasElementView elementView = GetHateoasView(currentResourceCode, element);
                embeddedList.Add(elementView);
            }

            HateoasLabeledLink parentLink = this.GetParentLink(parentResourceCode, currentResourceCode);

            var collectionView = new HateoasCollectionView(output.UsedFilter, output.TotalExisting, collectionLinks, collectionActions, embeddedList, parentLink);
            return collectionView;
        }

        public HateoasElementView GetHateoasView(string resourceCode, object viewModel)
        {
            string parentResourceCode = CurrentEndpoint.ResourceCode;
            HateoasLabeledLink parentLink = this.GetParentLink(parentResourceCode, resourceCode);

            var elemLinks = GetElementLinks(resourceCode, viewModel);
            var elemActions = GetHateoasActions(resourceCode, EndpointScope.Element, viewModel);

            var view = new HateoasElementView(elemLinks, elemActions, viewModel, parentLink);
            return view;
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
                    href = ReflectionUtils.ReplaceUrlTemplate(href, (element as IEnumerable<object>).First());
                }
                else
                {
                    href = ReflectionUtils.ReplaceUrlTemplate(href, element);
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

                if (valuesSource != null)
                    newAction.Href = ReflectionUtils.ReplaceUrlTemplate(newAction.Href, valuesSource);

                hateoasActionMap.Add(endpoint.EndpointName, newAction);
            }

            return hateoasActionMap;
        }

        protected Dictionary<string, string> GetCollectionLinks(string resourceCode, CollectionView output)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));
            if (output.UsedFilter == null) throw new ArgumentNullException(nameof(output.UsedFilter));
            if (output is null) throw new ArgumentNullException(nameof(output));

            // FILTRA METODOS GET DEL RESOURCECODE ACTUAL
            var usedFilter = output.UsedFilter;
            var relevantLinks = this.HateoasService.GetLinks(resourceCode, EndpointScope.Collection);
            int calculatedLastPage = (int)Math.Ceiling((double)output.TotalExisting / usedFilter.PageSize);

            // POR CADA UNO DE ESOS METODOS, CREA UN LINK. 
            // <NO> CREA LINK SELF
            var linkMap = new Dictionary<string, string>();
            foreach (Endpoint endpoint in relevantLinks)
            {
                if (CurrentEndpoint.EndpointName != endpoint.EndpointName)
                {
                    string href = "/" + endpoint.UrlTemplate;
                    linkMap.Add(endpoint.EndpointName, href);
                }
            }

            // AGREGA SELF DE FORMA CUSTOM PORQUE PUEDE HABER PARAMETROS INNECESARIOS EN EL FILTRO ACTUAL QUE MANDÓ EL USER
            linkMap.Add("self", GetCollectionLink(usedFilter));

            //if (CurrentEndpoint.Relation == EndpointRelation.Child)

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
    }
}
