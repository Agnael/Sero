using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
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
        public readonly IHateoasAuthorizator Authorizator;
        public readonly HateoasService HateoasService;

        public Endpoint CurrentEndpoint { get; private set; }
        public ActionDescriptor CurrentActionDescriptor { get; private set; }
        public string CurrentBaseUrlPath { get; set; }
        public IDictionary<string, object> ProvidedActionArguments { get; private set; }

        public HateoasFilter(
            HateoasService hateoasService,
            IHateoasAuthorizator authorizator)
        {
            this.HateoasService = hateoasService;

            if (authorizator == null)
            {
                // TODO: Loggear que tuvo que usarse el dummy
                this.Authorizator = new DummyAuthorizationService();
            }
            else
            {
                this.Authorizator = authorizator;
            }
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            //IFiltrableByOwner filter = 
            //    context
            //    .ActionArguments
            //    .Select(x => x.Value)
            //    .FirstOrDefault(x => x is IFiltrableByOwner) as IFiltrableByOwner;

            //if(filter != null)
            //{
            //    filter.SetRequiredOwnerId("asdlasjdVALORSITOO");
            //    context.ActionArguments.Add("ApiResourceOwnerId", "ownerid metido como arg extra en el diccionario");                               
            //}

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
                IResultView result = actionResult.GetClosestResultView();

                if (result is CollectionView)
                {
                    CollectionView output = (CollectionView)result;
                    (context.Result as ObjectResult).Value = GetHateoasCollectionView(output);
                    return;
                }
                else if(result is ElementView)
                {
                    ElementView output = (ElementView)result;
                    (context.Result as ObjectResult).Value = GetHateoasElementView(output.ViewModel);
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

        private HateoasCollectionView GetHateoasCollectionView(CollectionView output)
        {
            string parentResourceCode = CurrentEndpoint.ResourceCode;
            string currentResourceCode = output.ResourceCode;

            var collectionLinks = GetCollectionLinks(currentResourceCode, output);
            var collectionActions = GetCollectionActions(currentResourceCode);
            
            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in output.ViewModels)
            {
                HateoasElementView elementView = GetHateoasElementView(element);
                embeddedList.Add(elementView);
            }

            HateoasLabeledLink parentLink = this.GetParentLink(parentResourceCode, currentResourceCode);

            int totalExisting = output.TotalExisting;
            int totalPagesAvailable = (int)Math.Ceiling((double)totalExisting / output.UsedFilter.PageSize.Values.First());

            var allFiltersUsed = this.GetCollectionFilteringMap(output.UsedFilter);

            var collectionView = new HateoasCollectionView(totalExisting, totalPagesAvailable, allFiltersUsed, collectionLinks, collectionActions, embeddedList, parentLink);
            return collectionView;
        }

        public Dictionary<string, object> GetCollectionFilteringMap(FilteringOverview filter)
        {
            var allFiltersUsed = new Dictionary<string, object>();

            object pageValue =
                filter.Page.HasMultipleValues
                ? (object)filter.Page.Values
                : filter.Page.Values.First();

            allFiltersUsed.Add(filter.Page.Name, pageValue);

            object pageSizeValue =
                filter.PageSize.HasMultipleValues
                ? (object)filter.PageSize.Values
                : filter.PageSize.Values.First();

            allFiltersUsed.Add(filter.PageSize.Name, pageSizeValue);

            foreach(var extraCriteria in filter.AdditionalCriterias)
            {
                object extraCriteriaValue =
                    extraCriteria.HasMultipleValues
                    ? (object)extraCriteria.Values
                    : extraCriteria.Values.First();

                if(extraCriteriaValue != null
                    && extraCriteriaValue.GetType().IsEnum)
                {
                    extraCriteriaValue = extraCriteriaValue.ToString();
                }

                allFiltersUsed.Add(extraCriteria.Name, extraCriteriaValue);
            }

            return allFiltersUsed;
        }

        public HateoasElementView GetHateoasElementView(IApiResource resource)
        {
            string parentResourceCode = CurrentEndpoint.ResourceCode;
            HateoasLabeledLink parentLink = this.GetParentLink(parentResourceCode, resource.ApiResourceCode);

            var elemLinks = GetElementLinks(resource);
            var elemActions = GetElementActions(resource);

            var view = new HateoasElementView(elemLinks, elemActions, resource, parentLink);
            return view;
        }

        protected Dictionary<string, string> GetElementLinks(IApiResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));
            
            var linkMap = new Dictionary<string, string>();
            var relevantEndpoints = HateoasService.GetElementLinks(resource, Authorizator);

            foreach (Endpoint endpoint in relevantEndpoints)
            {
                string actionName = endpoint.EndpointName;

                if (endpoint.IsElementGetter)
                    actionName = "self";

                string href = "/" + endpoint.UrlTemplate;
                
                href = ReflectionUtils.ReplaceUrlTemplate(href, resource);
                                             
                linkMap.Add(actionName, href);
            }
            return linkMap;
        }

        protected Dictionary<string, HateoasAction> GetCollectionActions(string resourceCode)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));

            var hateoasActionMap = new Dictionary<string, HateoasAction>();
            var relevantEndpoints = HateoasService.GetCollectionActions(resourceCode, Authorizator);

            foreach (Endpoint endpoint in relevantEndpoints)
            {
                HateoasAction newAction = new HateoasAction();
                newAction.Method = endpoint.HttpMethod;
                newAction.Href = "/" + endpoint.UrlTemplate;
                
                hateoasActionMap.Add(endpoint.EndpointName, newAction);
            }

            return hateoasActionMap;
        }

        protected Dictionary<string, HateoasAction> GetElementActions(IApiResource resource)
        {
            if (resource == null) throw new ArgumentNullException(nameof(resource));

            var hateoasActionMap = new Dictionary<string, HateoasAction>();
            var relevantEndpoints = HateoasService.GetElementActions(resource, Authorizator);

            foreach (Endpoint endpoint in relevantEndpoints)
            {
                HateoasAction newAction = new HateoasAction();
                newAction.Method = endpoint.HttpMethod;
                newAction.Href = "/" + endpoint.UrlTemplate;

                newAction.Href = ReflectionUtils.ReplaceUrlTemplate(newAction.Href, resource);

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
            var relevantLinks = this.HateoasService.GetCollectionLinks(resourceCode, Authorizator);
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
                string paramValue = filter.Page.UrlFriendlyValueTransformerDefault(filter.Page.Values.First());

                urlBuilder.AddParam(paramKey, paramValue);
            }

            if (filter.PageSize.IsModified)
            {
                string paramKey = filter.PageSize.Name.ToLower();
                string paramValue = filter.PageSize.UrlFriendlyValueTransformerDefault(filter.PageSize.Values.First());

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
