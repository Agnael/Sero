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
        public readonly HateoasService HateoasService;
        public readonly IAuthorizationService AuthorizationService;

        public Endpoint CurrentEndpoint { get; private set; }
        public ActionDescriptor CurrentActionDescriptor { get; private set; }
        public string CurrentBaseUrlPath { get; set; }
        public IDictionary<string, object> ProvidedActionArguments { get; private set; }

        public HateoasFilter(
            HateoasService hateoasService,
            IAuthorizationService authorizationService)
        {
            this.HateoasService = hateoasService;

            if (authorizationService == null)
            {
                // TODO: Loggear que tuvo que usarse el dummy
                this.AuthorizationService = new DummyAuthorizationService();
            }
            else
            {
                this.AuthorizationService = authorizationService;
            }
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
            IActionResult actionResult = context.Result;

            if (actionResult is ObjectResult
                && !(actionResult is BadRequestObjectResult))
            {
                IResultView result = null;

                if(actionResult is CreatedResult)
                {
                    result = ((actionResult as CreatedResult).Value as ObjectResult).Value as IResultView;
                }
                else
                {
                    result = (actionResult as ObjectResult).Value as IResultView;
                }

                if (result is CollectionView)
                {
                    CollectionView output = (CollectionView)result;
                    (context.Result as ObjectResult).Value = GetHateoasView(output);
                    return;
                }
                else if(result is ElementView)
                {
                    ElementView output = (ElementView)result;
                    (context.Result as ObjectResult).Value = GetHateoasView(output.ResourceCode, output.ViewModel);
                    return;
                }
                else
                {
                    throw new Exception("Can't generate a HATEOAS context for this IActionResult");
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

            int totalExisting = output.TotalExisting;
            int totalPagesAvailable = (int)Math.Ceiling((double)totalExisting / output.UsedFilter.PageSize.Values.First());

            var collectionView = new HateoasCollectionView(totalExisting, totalPagesAvailable, collectionLinks, collectionActions, embeddedList, parentLink);
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
            var relevantEndpoints = HateoasService.GetLinks(resourceCode, EndpointScope.Element, AuthorizationService.IsAuthorized);

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
            var relevantEndpoints = HateoasService.GetActions(resourceCode, scope, AuthorizationService.IsAuthorized);

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
            var relevantLinks = this.HateoasService.GetLinks(resourceCode, EndpointScope.Collection, AuthorizationService.IsAuthorized);
            int currentPageSize = usedFilter.PageSize.Values.First();
            int calculatedLastPage = (int)Math.Ceiling((double)output.TotalExisting / currentPageSize);

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

            // AGREGA LINKS DE PAGINACION
            int currentPage = usedFilter.Page.Values.First();

            if (currentPage >= 1 && currentPage <= calculatedLastPage)
            {
                if (currentPage > 1)
                {
                    int prevPage =
                        currentPage > 1 
                        ? currentPage - 1 
                        : currentPage;

                    var prevFilter = usedFilter.CopyAsPage(prevPage);
                    string urlPrev = GetCollectionLink(prevFilter);
                    linkMap.Add("prev", urlPrev);

                    var firstFilter = usedFilter.CopyAsPage(1);
                    string urlFirst = GetCollectionLink(firstFilter);
                    linkMap.Add("first", urlFirst);
                }

                if (currentPage < calculatedLastPage)
                {
                    int nextPage =
                        currentPage < calculatedLastPage 
                        ? currentPage + 1 
                        : currentPage;
                    var nextFilter = usedFilter.CopyAsPage(nextPage);
                    string urlNext = GetCollectionLink(nextFilter);
                    linkMap.Add("next", urlNext);

                    var lastFilter = usedFilter.CopyAsPage(calculatedLastPage);
                    string urlLast = GetCollectionLink(lastFilter);
                    linkMap.Add("last", urlLast);
                }
            }

            return linkMap;
        }

        protected string GetCollectionLink(FilteringOverview filter)
        {
            var urlBuilder = new UrlBuilder<ICollectionFilter>(this.CurrentBaseUrlPath);

            if (filter.Page.IsModified)
            {
                string paramKey = filter.Page.Name.ToLower();
                string paramValue = filter.Page.UrlFriendlyValueTransformer(filter.Page.Values.First());

                urlBuilder.AddParam(paramKey, paramValue);
            }

            if (filter.PageSize.IsModified)
            {
                string paramKey = filter.PageSize.Name.ToLower();
                string paramValue = filter.PageSize.UrlFriendlyValueTransformer(filter.PageSize.Values.First());

                urlBuilder.AddParam(paramKey, paramValue);
            }

            foreach(var additionalCriteria in filter.AdditionalCriterias)
            {
                if (additionalCriteria.IsModified)
                {
                    string paramKey = additionalCriteria.Name.ToLower();

                    foreach(var value in additionalCriteria.Values)
                    {
                        string paramValue = additionalCriteria.UrlFriendlyValueTransformer(value);
                        urlBuilder.AddParam(paramKey, paramValue);
                    }
                }
            }

            string url = urlBuilder.Build();
            return url;
        }
    }
}
