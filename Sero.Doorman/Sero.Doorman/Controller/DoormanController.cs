using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Sero.Core.Mvc;
using Sero.Core.Mvc.Models;
using Sero.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sero.Doorman.Controller
{
    public abstract class DoormanController<TResource> : ControllerBase
    {
        public readonly RequestUtils RequestUtils;

        public DoormanController(
            RequestUtils requestUtils)
        {
            this.RequestUtils = requestUtils;
        }

        protected DoormanActionAttribute _doormanActionAttr;
        protected DoormanActionAttribute DoormanActionAttr
        {
            get
            {
                if (_doormanActionAttr == null)
                {
                    if (this.ControllerContext == null) return null;

                    var actionDescriptor = this.ControllerContext.ActionDescriptor;
                    if (actionDescriptor == null) return null;

                    var endpointMeta = actionDescriptor.EndpointMetadata;
                    if (endpointMeta == null) return null;

                    _doormanActionAttr = (DoormanActionAttribute)endpointMeta.FirstOrDefault(x => x is DoormanActionAttribute);
                }

                return _doormanActionAttr;
            }
        }

        protected async Task<IActionResult> ValidationErrorAsync()
        {
            var validationErrorView = new ValidationErrorView();
            validationErrorView.ErrorMap = ModelState.ToDictionary(
               kvp => kvp.Key,
               kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
           );

            return StatusCode(StatusCodes.Status400BadRequest, validationErrorView);
        }

        protected async Task<IActionResult> UnknownErrorAsync()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        protected async void SetStatusCode(int httpStatusCode)
        {
            this.HttpContext.Response.StatusCode = httpStatusCode;
        }

        protected async Task<IActionResult> SuccessAsync(TResource resource)
        {
            var view = new ElementView();
            //view._links = GetMetaLinks(this.DoormanActionAttr.ResourceCode, this.DoormanActionAttr.ActionScope);
            //view._links.Add("_self", GetSelfMetaLink());

            view._embeded = resource;

            return new ObjectResult(view);
        }

        protected CollectionFilter GetUsedCollectionFilter()
        {
            if(RequestUtils.CurrentActionArguments == null
                || RequestUtils.CurrentActionArguments.Count == 0)
            {
                throw new ListMethodWithMissingFilter();
            }

            CollectionFilter usedFilter = 
                (CollectionFilter)RequestUtils
                .CurrentActionArguments
                .First()
                .Value;

            return usedFilter;
        }

        protected Dictionary<string, string> GetHateoasLinks(string resourceCode, ActionScope scope)
        {
            Dictionary<string, string> linkMap = new Dictionary<string, string>();

            foreach (ControllerActionDescriptor action in this.RequestUtils.ActionDescriptors)
            {
                string actionHttpMethod = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .SelectMany(x => x.HttpMethods)
                            .Distinct()
                            .First();

                if (actionHttpMethod.ToUpper() == "GET"
                    && ControllerContext.ActionDescriptor.ActionName != action.ActionName)
                {
                    var doormanAttr = (DoormanActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is DoormanActionAttribute);

                    if (doormanAttr.ResourceCode == resourceCode
                        && doormanAttr.ActionScope == scope)
                    {
                        ResourceLink newLink = new ResourceLink();

                        var httpMethodAttr = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .First();
                        newLink.UrlTemplate = httpMethodAttr.Template;

                        newLink.Verb = actionHttpMethod;

                        string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);
                        linkMap.Add(actionName, httpMethodAttr.Template);
                    }
                }
            }

            return linkMap;
        }

        [NonAction]
        public string GetCollectionEndpointUrl(
            CollectionFilter filter, 
            int calculatedLastPage,
            bool tryPreviousPage = false, 
            bool tryNextPage = false,
            bool tryFirst = false,
            bool tryLast = false)
        {
            var relevantParams = new Dictionary<string, string>();

            if (tryPreviousPage)
                filter.Page = filter.Page > 1 ? filter.Page - 1 : filter.Page;

            if (tryNextPage)
                filter.Page = filter.Page < calculatedLastPage ? filter.Page+1 : filter.Page;

            if (tryFirst)
                filter.Page = 1;

            if (tryLast)
                filter.Page = calculatedLastPage;

            if (filter.TextSearch != null)
                relevantParams.Add(nameof(filter.TextSearch), filter.TextSearch);

            if (filter.OrderBy.ToLower() != Order.ASC.ToLower())
                relevantParams.Add(nameof(filter.OrderBy), filter.OrderBy);

            if (filter.Page > 1)
                relevantParams.Add(nameof(filter.Page), filter.Page.ToString());

            if (filter.PageSize != 10)
                relevantParams.Add(nameof(filter.PageSize), filter.PageSize.ToString());

            if (filter.SortBy.ToLower() != filter.GetDefaultSortingField().ToLower())
                relevantParams.Add(nameof(filter.SortBy), filter.SortBy.ToString());

            List<string> qsParts = new List<string>();
            string qs = null;

            foreach(var param in relevantParams)
                qsParts.Add(string.Format("{0}={1}", param.Key.ToLower(), param.Value));

            if(qsParts.Count > 0)
            {
                string joinedParams = string.Join('&', qsParts.ToArray());
                qs = string.Format("?{0}", joinedParams);
            }

            string urlWithoutQs = HttpContext.Request.Path;
            string resultUrl = urlWithoutQs + qs;

            return resultUrl;
        }

        protected async Task<IActionResult> SuccessAsync(IEnumerable<TResource> resourceList, int totalAvailableElementCount)
        {
            CollectionFilter usedFilter = GetUsedCollectionFilter();

            int calculatedLastPage = (int)Math.Ceiling((double)totalAvailableElementCount / usedFilter.PageSize);

            var view = new CollectionView(usedFilter, totalAvailableElementCount);
            view._links = GetHateoasLinks(DoormanActionAttr.ResourceCode, ActionScope.Collection);

            string urlSelf = GetCollectionEndpointUrl(usedFilter, calculatedLastPage);
            view._links.Add("self", urlSelf);

            if(usedFilter.Page > 1)
            {
                string urlPrev = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, true);
                view._links.Add("prev", urlPrev);

                string urlFirst = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, false, true);
                view._links.Add("first", urlFirst);
            }

            if(usedFilter.Page < calculatedLastPage)
            {
                string urlNext = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, true);
                view._links.Add("next", urlNext);

                string urlLast = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, false, false, true);
                view._links.Add("last", urlLast);
            }

            List<SuccessView> embeddedList = new List<SuccessView>();

            foreach (TResource resource in resourceList)
            {
                SuccessView embeddedResource = new ElementView();
                embeddedResource._embeded = resource;

                embeddedList.Add(embeddedResource);
            }
            view._embeded = embeddedList;

            return new ObjectResult(view);
        }

        protected object GetSelfMetaLink()
        {
            Dictionary<string, object> qsParamMap = new Dictionary<string, object>();

            if (HttpContext.Request.Method == "GET"
                && HttpContext.Request.QueryString.HasValue)
            {
                NameValueCollection qsNameValues = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.Value);

                foreach (string qsKey in qsNameValues.AllKeys)
                {
                    qsParamMap.Add(qsKey, qsNameValues.GetValues(qsKey));
                }
            }

            object selfData = new
            {
                href = HttpContext.Request.Path.Value,
                verb = HttpContext.Request.Method,
                data = qsParamMap
            };

            return selfData;
        }

        protected Dictionary<string, object> GetMetaLinks(string resourceCode, ActionScope scope)
        {
            Dictionary<string, object> linkMap = new Dictionary<string, object>();

            foreach (ControllerActionDescriptor action in this.RequestUtils.ActionDescriptors)
            {
                var doormanAttr = (DoormanActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is DoormanActionAttribute);

                if (doormanAttr != null
                    && doormanAttr.ResourceCode == resourceCode
                    && doormanAttr.ActionScope == scope
                    /*&& doormanAttr.ActionCode != this.DoormanActionAttr.ActionCode*/)
                {
                    ResourceLink newLink = new ResourceLink();

                    var httpMethodAttr = action
                        .EndpointMetadata
                        .OfType<HttpMethodAttribute>()
                        .First();
                    newLink.UrlTemplate = httpMethodAttr.Template;

                    newLink.Verb = action
                        .EndpointMetadata
                        .OfType<HttpMethodAttribute>()
                        .SelectMany(x => x.HttpMethods)
                        .Distinct()
                        .First();
                }
            }

            return linkMap;
        }
    }
}
