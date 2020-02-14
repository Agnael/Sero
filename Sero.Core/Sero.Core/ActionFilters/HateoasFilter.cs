using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var elemActions = GetHateoasActions(resource, resourceCode, ActionScope.Element);

            var view = new HateoasElementView(elemLinks, elemActions, resource);
            return view;
        }

        public HateoasCollectionView GetCollectionView(CollectionResult collection)
        {
            // Para que funcione, TODOS los GETs que se hagan de collections, deben tomar SOLO un parámetro, que debe heredar de la clase CollectionFilter
            CollectionFilter filter = collection.UsedFilter;
            string resourceCode = CurrentHateoasAttr.ResourceCode;
            var collectionActions = GetHateoasActions(null, resourceCode, ActionScope.Collection);
            var collectionLinks = GetCollectionLinks(resourceCode, filter);

            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in collection.ElementsToReturn)
            {
                HateoasElementView elementView = GetElementView(element);
                embeddedList.Add(elementView);
            }

            var hateoasCollectionView = new HateoasCollectionView(filter, collection.TotalElementsExisting, collectionLinks, collectionActions, embeddedList);
            return hateoasCollectionView;
        }

        //[NonAction]
        //public string GetCollectionEndpointUrl(
        //    CollectionFilter filter,
        //    int calculatedLastPage,
        //    bool tryPreviousPage = false,
        //    bool tryNextPage = false,
        //    bool tryFirst = false,
        //    bool tryLast = false)
        //{
        //    var relevantParams = new Dictionary<string, string>();

        //    CollectionFilter newFilter = filter.Copy();

        //    if (tryPreviousPage)
        //        newFilter.Page = filter.Page > 1 ? filter.Page - 1 : filter.Page;

        //    if (tryNextPage)
        //        newFilter.Page = filter.Page < calculatedLastPage ? filter.Page + 1 : filter.Page;

        //    if (tryFirst)
        //        newFilter.Page = 1;

        //    if (tryLast)
        //        newFilter.Page = calculatedLastPage;

        //    if (!newFilter.IsDefaultTextSearch())
        //        relevantParams.Add(nameof(newFilter.TextSearch), newFilter.TextSearch);

        //    if (!newFilter.IsDefaultPage())
        //        relevantParams.Add(nameof(newFilter.Page), newFilter.Page.ToString());

        //    if (!newFilter.IsDefaultPageSize())
        //        relevantParams.Add(nameof(newFilter.PageSize), newFilter.PageSize.ToString());

        //    if (!newFilter.IsDefaultSortBy())
        //        relevantParams.Add(nameof(newFilter.SortBy), newFilter.SortBy.ToString());

        //    if (!newFilter.IsDefaultOrderBy())
        //        relevantParams.Add(nameof(newFilter.OrderBy), newFilter.OrderBy);

        //    List<string> qsParts = new List<string>();
        //    string qs = null;

        //    foreach (var param in relevantParams)
        //        qsParts.Add(string.Format("{0}={1}", param.Key.ToLower(), param.Value));

        //    if (qsParts.Count > 0)
        //    {
        //        string joinedParams = string.Join('&', qsParts.ToArray());
        //        qs = string.Format("?{0}", joinedParams);
        //    }

        //    string urlWithoutQs = HttpContext.Request.Path;
        //    string resultUrl = urlWithoutQs + qs;

        //    return resultUrl;
        //}

        protected Dictionary<string, string> GetCollectionLinks(string resourceCode, CollectionFilter usedFiler)
        {
            if (string.IsNullOrEmpty(resourceCode)) throw new ArgumentNullException(nameof(resourceCode));
            if (usedFiler == null) throw new ArgumentNullException(nameof(usedFiler));

            // FILTRA METODOS GET DEL RESOURCECODE ACTUAL
            var relevantLinks = ActionDescriptors.Where(x => x.IsLinkFor(resourceCode, ActionScope.Collection));

            // POR CADA UNO DE ESOS METODOS, CREA UN LINK. 
            // <NO> VA A CREAR LINK SELF
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

            // AGREGA LINKS DE PAGINACION



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

            }





            foreach (ActionDescriptor action in httpGetActions)
            {
                bool isMainGetter = action.IsElementGetter();

                if (isMainGetter || CurrentAction.DisplayName != action.DisplayName)
                {
                    var hateoasAttr = action.GetHateoasAttribute();

                    if (hateoasAttr.ResourceCode == resourceCode
                        && hateoasAttr.ActionScope == ActionScope.Element)
                    {
                        var httpMethodAttr = action.GetHttpMethodAttribute();
                        string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.DisplayName);

                        if (isMainGetter)
                            actionName = "self";

                        string href = "/" + ReplaceUrlTemplate(httpMethodAttr.Template, element);
                        linkMap.Add(actionName, href);
                    }
                }
            }
            return linkMap;
        }

        //protected Dictionary<string, string> GetHateoasLinks(object element, string resourceCode, ActionScope scope)
        //{
        //    var linkMap = new Dictionary<string, string>();

        //    foreach (ControllerActionDescriptor action in this.ActionDescriptors)
        //    {
        //        string actionHttpMethod = action.GetHttpMethodValue();
        //        bool isGetter = action.IsElementGetter();

        //        if (actionHttpMethod == "GET"
        //            && (isGetter || CurrentAction.DisplayName != action.ActionName))
        //        {
        //            var hateoasAttr = action.GetHateoasAttribute();

        //            if (hateoasAttr.ResourceCode == resourceCode
        //                && hateoasAttr.ActionScope == ActionScope.Element)
        //            {
        //                var httpMethodAttr = action.GetHttpMethodAttribute();

        //                string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);

        //                if (action.IsElementGetter() || CurrentAction.DisplayName == action.DisplayName)
        //                    actionName = "self";

        //                string href = "/" + httpMethodAttr.Template;

        //                if (element != null)
        //                    href = ReplaceUrlTemplate(href, element);

        //                linkMap.Add(actionName, href);
        //            }
        //        }
        //    }

        //    return linkMap;
        //}

        protected Dictionary<string, HateoasAction> GetHateoasActions(object element, string resourceCode, ActionScope scope)
        {
            var actionMap = new Dictionary<string, HateoasAction>();

            foreach (ControllerActionDescriptor action in this.ActionDescriptors)
            {
                string actionHttpMethod = action.GetHttpMethodValue();
                bool isGetter = action.IsElementGetter();

                if (actionHttpMethod != "GET"
                    && (isGetter || CurrentAction.DisplayName != action.ActionName))
                {
                    var doormanAttr = action.GetHateoasAttribute();

                    if (doormanAttr.ResourceCode == resourceCode
                        && doormanAttr.ActionScope == scope)
                    {
                        var httpMethodAttr = action.GetHttpMethodAttribute();

                        string actionName = CasingUtil.UpperCamelCaseToLowerUnderscore(action.ActionName);
                        HateoasAction newAction = new HateoasAction();
                        newAction.Method = actionHttpMethod.ToUpper();
                        newAction.Href = "/" + httpMethodAttr.Template;

                        if (element != null)
                            newAction.Href = ReplaceUrlTemplate(newAction.Href, element);

                        actionMap.Add(actionName, newAction);
                    }
                }
            }
            return actionMap;
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
