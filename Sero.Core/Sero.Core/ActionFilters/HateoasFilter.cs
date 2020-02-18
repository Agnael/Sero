using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Sero.Core
{
    public class HateoasFilter : IActionFilter
    {
        public ActionDescriptor CurrentAction { get; private set; }
        public HateoasActionAttribute CurrentHateoasAttr { get; private set; }
        public string CurrentResourceCode { get; set; }

        /// <summary>
        ///     Current URL <WITHOUT> the querystring part
        /// </summary>
        public string CurrentBaseUrlPath { get; set; }

        public readonly IReadOnlyList<ActionDescriptor> ActionDescriptors;
        public IDictionary<string, object> ProvidedActionArguments { get; private set; }

        public HateoasFilter(IActionDescriptorCollectionProvider actionDesccriptorCollectionProvider)
        {
            this.ActionDescriptors = actionDesccriptorCollectionProvider.ActionDescriptors.Items;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {        
            this.ProvidedActionArguments = context.ActionArguments;
            this.CurrentAction = context.ActionDescriptor;
            this.CurrentBaseUrlPath = context.HttpContext.Request.Path;

            this.CurrentHateoasAttr =
                (HateoasActionAttribute)this.CurrentAction
                .EndpointMetadata
                .FirstOrDefault(x => x is HateoasActionAttribute);
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
            string resourceCode = CurrentHateoasAttr.ResourceCode;
            var elemLinks = GetElementLinks(resourceCode, resource);
            var elemActions = GetHateoasActions(resourceCode, ActionScope.Element, resource);

            var view = new HateoasElementView(elemLinks, elemActions, resource);
            return view;
        }

        public HateoasCollectionView GetCollectionView(CollectionResult collection)
        {
            // Para que funcione, TODOS los GETs que se hagan de collections, deben tomar SOLO un parámetro, que debe heredar de la clase CollectionFilter
            CollectionFilter filter = collection.UsedFilter;
            string resourceCode = CurrentHateoasAttr.ResourceCode;
            var collectionLinks = GetCollectionLinks(resourceCode, collection);
            var collectionActions = GetHateoasActions(resourceCode, ActionScope.Collection);

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
            var relevantLinks = ActionDescriptors.Where(x => x.IsLinkFor(resourceCode, ActionScope.Collection));
            int calculatedLastPage = (int)Math.Ceiling((double)processedResult.TotalElementsExisting / usedFilter.PageSize);

            // POR CADA UNO DE ESOS METODOS, CREA UN LINK. 
            // <NO> CREA LINK SELF
            var linkMap = new Dictionary<string, string>();
            foreach(ActionDescriptor action in relevantLinks)
            {
                if (CurrentAction.DisplayName != action.DisplayName)
                {
                    var httpMethodAttr = action.GetHttpMethodAttribute();
                    string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.DisplayName);
                    string href = "/" + httpMethodAttr.Template;

                    linkMap.Add(actionName, href);
                }
            }

            // AGREGA SELF DE FORMA CUSTOM PORQUE PUEDE HABER PARAMETROS INNECESARIOS EN EL FILTRO ACTUAL QUE MANDÓ EL USER
            linkMap.Add("self", GetCollectionLink(usedFilter));

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
            var relevantActions = ActionDescriptors.Where(x => x.IsLinkFor(resourceCode, ActionScope.Element));

            foreach(ActionDescriptor action in relevantActions)
            {
                var httpMethodAttr = action.GetHttpMethodAttribute();
                string actionName = "";

                if (action.IsElementGetter())
                    actionName = "self";
                else
                    actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.DisplayName);

                string href = "/" + ReplaceUrlTemplate(httpMethodAttr.Template, element);
                linkMap.Add(actionName, href);
            }
            return linkMap;
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(string resourceCode, ActionScope scope)
        {
            return this.GetHateoasActions(resourceCode, scope, null);
        }

        protected Dictionary<string, HateoasAction> GetHateoasActions(string resourceCode, ActionScope scope, object valuesSource)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));

            var hateoasActionMap = new Dictionary<string, HateoasAction>();
            var relevantMvcActions = ActionDescriptors.Where(x => x.IsActionFor(resourceCode, scope));

            foreach (ControllerActionDescriptor action in relevantMvcActions)
            {
                var httpMethodAttr = action.GetHttpMethodAttribute();
                string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);

                HateoasAction newAction = new HateoasAction();
                newAction.Method = httpMethodAttr.HttpMethods.First();
                newAction.Href = "/" + httpMethodAttr.Template;

                if(valuesSource != null)
                    newAction.Href = ReplaceUrlTemplate(newAction.Href, valuesSource);

                hateoasActionMap.Add(actionName, newAction);
            }

            return hateoasActionMap;
        }

        protected string ReplaceUrlTemplate(string urlTemplate, object valuesSource)
        {
            string result = urlTemplate;

            if (valuesSource != null)
            {
                // Este regex trae el texto ENTRE llaves {} pero sin las llaves
                Regex regex = new Regex(@"(?<={)(.*?)(?=})");
                var match = regex.Match(result);

                if (match.Success)
                {
                    var elementPropList = valuesSource.GetType().GetProperties();

                    foreach (Group matchedGroup in match.Groups)
                    {
                        string urlParam = matchedGroup.Value?.ToLower();
                        PropertyInfo propInfo = elementPropList.FirstOrDefault(x => x.Name.ToLower() == urlParam);
                        object foundValue = propInfo.GetValue(valuesSource, null);

                        if (propInfo != null)
                            result = result.Replace(string.Format("{{{0}}}", urlParam), foundValue?.ToString());
                    }
                }
            }

            return result;
        }
    }
}
