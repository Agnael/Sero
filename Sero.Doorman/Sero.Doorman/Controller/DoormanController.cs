using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Sero.Core.Mvc;
using Sero.Core.Mvc.Models;
using Sero.Core.Utils;
using Sero.Doorman.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Sero.Doorman.Controller
{
    public abstract class DoormanController<TResource> : ControllerBase where TResource : class
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

        protected bool? _hasDoormanElementGetterAttr = null;
        protected bool HasDoormanElementGetterAttr
        {
            get
            {
                if (_hasDoormanElementGetterAttr == null)
                {
                    if (this.ControllerContext == null)
                    {
                        _hasDoormanElementGetterAttr = false;
                        return false;
                    }

                    var actionDescriptor = this.ControllerContext.ActionDescriptor;
                    if (actionDescriptor == null)
                    {
                        _hasDoormanElementGetterAttr = false;
                        return false;
                    }

                    var endpointMeta = actionDescriptor.EndpointMetadata;
                    if (endpointMeta == null)
                    {
                        _hasDoormanElementGetterAttr = false;
                        return false;
                    }

                    _hasDoormanElementGetterAttr = endpointMeta.Any(x => x is DoormanElementGetterAttribute);
                }

                return _hasDoormanElementGetterAttr.Value;
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

        protected async Task<IActionResult> ElementAsync(TResource resource)
        {
            var view = GetElementView(resource);
            return new ObjectResult(view);
        }

        protected ElementView GetElementView(TResource resource)
        {
            var view = new ElementView();
            view._actions = GetHateoasActions(resource, DoormanActionAttr.ResourceCode, ActionScope.Element);
            view._links = GetHateoasLinks(resource, DoormanActionAttr.ResourceCode, ActionScope.Element);
            view._embeded = resource;

            return view;
        }

        protected async Task<IActionResult> CollectionAsync(IEnumerable<TResource> resourceList, int totalAvailableElementCount)
        {
            CollectionFilter usedFilter = GetUsedCollectionFilter();

            int calculatedLastPage = (int)Math.Ceiling((double)totalAvailableElementCount / usedFilter.PageSize);

            var view = new CollectionView(usedFilter, totalAvailableElementCount);
            view._actions = GetHateoasActions(DoormanActionAttr.ResourceCode, ActionScope.Collection);
            view._links = GetHateoasLinks(DoormanActionAttr.ResourceCode, ActionScope.Collection);

            string urlSelf = GetCollectionEndpointUrl(usedFilter, calculatedLastPage);
            view._links.Add("self", urlSelf);
            AddPaginationHateoasLinks(view._links, usedFilter, calculatedLastPage);

            List<SuccessView> embeddedList = new List<SuccessView>();

            foreach (TResource resource in resourceList)
            {
                SuccessView elementView = GetElementView(resource);
                embeddedList.Add(elementView);
            }
            view._embeded = embeddedList;

            return new ObjectResult(view);
        }

        protected void AddPaginationHateoasLinks(Dictionary<string, string> linkMap, CollectionFilter usedFilter, int calculatedLastPage)
        {
            if (usedFilter.Page >= 1 && usedFilter.Page <= calculatedLastPage)
            {
                if (usedFilter.Page > 1)
                {
                    string urlPrev = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, true);
                    linkMap.Add("prev", urlPrev);

                    string urlFirst = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, false, true);
                    linkMap.Add("first", urlFirst);
                }

                if (usedFilter.Page < calculatedLastPage)
                {
                    string urlNext = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, true);
                    linkMap.Add("next", urlNext);

                    string urlLast = GetCollectionEndpointUrl(usedFilter, calculatedLastPage, false, false, false, true);
                    linkMap.Add("last", urlLast);
                }
            }
        }

        protected Dictionary<string, string> GetHateoasLinks(string resourceCode, ActionScope scope)
        {
            return GetHateoasLinks(null, resourceCode, scope);
        }

        protected Dictionary<string, string> GetHateoasLinks(TResource element, string resourceCode, ActionScope scope)
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
                    && (this.HasDoormanElementGetterAttr 
                        || ControllerContext.ActionDescriptor.ActionName != action.ActionName))
                {
                    var doormanAttr = (DoormanActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is DoormanActionAttribute);

                    if (doormanAttr.ResourceCode == resourceCode
                        && doormanAttr.ActionScope == scope)
                    {
                        var doormanElementGetterAttr = action.EndpointMetadata.FirstOrDefault(x => x is DoormanElementGetterAttribute);
                        var httpMethodAttr = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .First();

                        string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);

                        if (doormanElementGetterAttr != null)
                            actionName = "self";

                        string href = "/" + httpMethodAttr.Template;

                        if (element != null)
                            href = ReplaceUrlTemplate(href, element);

                        linkMap.Add(actionName, href);
                    }
                }
            }

            return linkMap;
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(string resourceCode, ActionScope scope)
        {
            return GetHateoasActions(null, resourceCode, scope);
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(TResource element, string resourceCode, ActionScope scope)
        {
            Dictionary<string, HateoasAction> actionMap = new Dictionary<string, HateoasAction>();

            foreach (ControllerActionDescriptor action in this.RequestUtils.ActionDescriptors)
            {
                string actionHttpMethod = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .SelectMany(x => x.HttpMethods)
                            .Distinct()
                            .First();

                if (actionHttpMethod.ToUpper() != "GET"
                    && (this.HasDoormanElementGetterAttr 
                        || ControllerContext.ActionDescriptor.ActionName != action.ActionName))
                {
                    var doormanAttr = (DoormanActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is DoormanActionAttribute);

                    if (doormanAttr.ResourceCode == resourceCode
                        && doormanAttr.ActionScope == scope)
                    {
                        var httpMethodAttr = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .First();

                        string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);
                        HateoasAction newAction = new HateoasAction();
                        newAction.Method = actionHttpMethod.ToUpper();
                        newAction.Href = "/"+ httpMethodAttr.Template;

                        if (element != null)
                            newAction.Href = ReplaceUrlTemplate(newAction.Href, element);

                        actionMap.Add(actionName, newAction);
                    }
                }
            }

            return actionMap;
        }

        protected string ReplaceUrlTemplate(string urlTemplate, TResource element)
        {
            string result = urlTemplate;

            if (element != null)
            {
                // Este regex trae el texto ENTRE llaves {} pero sin las llaves
                Regex regex = new Regex(@"(?<={)(.*?)(?=})");
                var match = regex.Match(result);

                if (match.Success)
                {
                    var elementPropList = typeof(TResource).GetProperties();

                    foreach (Group matchedGroup in match.Groups)
                    {
                        string urlParam = matchedGroup.Value?.ToLower();
                        PropertyInfo propInfo = elementPropList.FirstOrDefault(x => x.Name.ToLower() == urlParam);
                        object foundValue = propInfo.GetValue(element, null);

                        if (propInfo != null)
                            result = result.Replace(string.Format("{{{0}}}", urlParam), foundValue?.ToString());
                    }
                }
            }

            return result;
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

            CollectionFilter newFilter = filter.Copy();

            if (tryPreviousPage)
                newFilter.Page = filter.Page > 1 ? filter.Page - 1 : filter.Page;

            if (tryNextPage)
                newFilter.Page = filter.Page < calculatedLastPage ? filter.Page + 1 : filter.Page;

            if (tryFirst)
                newFilter.Page = 1;

            if (tryLast)
                newFilter.Page = calculatedLastPage;

            if (!newFilter.IsDefaultTextSearch())
                relevantParams.Add(nameof(newFilter.TextSearch), newFilter.TextSearch);

            if (!newFilter.IsDefaultPage())
                relevantParams.Add(nameof(newFilter.Page), newFilter.Page.ToString());

            if (!newFilter.IsDefaultPageSize())
                relevantParams.Add(nameof(newFilter.PageSize), newFilter.PageSize.ToString());

            if (!newFilter.IsDefaultSortBy())
                relevantParams.Add(nameof(newFilter.SortBy), newFilter.SortBy.ToString());

            if (!newFilter.IsDefaultOrderBy())
                relevantParams.Add(nameof(newFilter.OrderBy), newFilter.OrderBy);

            List<string> qsParts = new List<string>();
            string qs = null;

            foreach (var param in relevantParams)
                qsParts.Add(string.Format("{0}={1}", param.Key.ToLower(), param.Value));

            if (qsParts.Count > 0)
            {
                string joinedParams = string.Join('&', qsParts.ToArray());
                qs = string.Format("?{0}", joinedParams);
            }

            string urlWithoutQs = HttpContext.Request.Path;
            string resultUrl = urlWithoutQs + qs;

            return resultUrl;
        }
    }
}
