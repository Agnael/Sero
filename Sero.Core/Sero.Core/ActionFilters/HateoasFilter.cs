﻿using Microsoft.AspNetCore.Mvc;
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
            var elemLinks = GetHateoasLinks(resource, resourceCode, ActionScope.Element);
            var elemActions = GetHateoasActions(resource, resourceCode, ActionScope.Element);

            var view = new HateoasElementView(elemLinks, elemActions, resource);
            return view;
        }

        public HateoasCollectionView GetCollectionView(CollectionResult collection)
        {
            // Para que funcione, TODOS los GETs que se hagan de collections, deben tomar SOLO un parámetro, que debe heredar de la clase CollectionFilter
            CollectionFilter filter = collection.UsedFilter;
            string resourceCode = CurrentHateoasAttr.ResourceCode;
            var collectionLinks = GetHateoasLinks(null, resourceCode, ActionScope.Collection);
            var collectionActions = GetHateoasActions(null, resourceCode, ActionScope.Collection);

            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in collection.ElementsToReturn)
            {
                HateoasElementView elementView = GetElementView(element);
                embeddedList.Add(elementView);
            }

            var hateoasCollectionView = new HateoasCollectionView(filter, collection.TotalElementsExisting, collectionLinks, collectionActions, embeddedList);
            return hateoasCollectionView;
        }

        protected void HandleCollection(ActionExecutedContext context, int statusCode)
        {
            ObjectResult actionResult = context.Result as ObjectResult;
            CollectionResult collectionResult = actionResult.Value as CollectionResult;

            string resourceCode = CurrentHateoasAttr.ResourceCode;

            // Para que funcione, TODOS los GETs que se hagan de collections, deben tomar SOLO un parámetro, que debe heredar de la clase CollectionFilter
            CollectionFilter filter = collectionResult.UsedFilter;
            int calculatedLastPage = (int)Math.Ceiling((double)collectionResult.TotalElementsExisting / filter.PageSize);

            Dictionary<string, string> collectionLinks = GetHateoasLinks(null, resourceCode, ActionScope.Collection);
            Dictionary<string, HateoasAction> collectionActions = GetHateoasActions(null, resourceCode, ActionScope.Collection);

            List<HateoasElementView> embeddedList = new List<HateoasElementView>();
            foreach (var element in collectionResult.ElementsToReturn)
            {
                Dictionary<string, string> elementLinks = GetHateoasLinks(element, resourceCode, ActionScope.Element);
                Dictionary<string, HateoasAction> elementActions = GetHateoasActions(element, resourceCode, ActionScope.Element);
                HateoasElementView elementView = new HateoasElementView(elementLinks, elementActions, element);
                embeddedList.Add(elementView);
            }

            var hateoasCollectionView = new HateoasCollectionView(filter, collectionResult.TotalElementsExisting, collectionLinks, collectionActions, embeddedList);

            var newResult = new JsonResult(hateoasCollectionView);
            newResult.StatusCode = statusCode;
            context.Result = newResult;
        }

        protected Dictionary<string, string> GetHateoasLinks(object element, string resourceCode, ActionScope scope)
        {
            Dictionary<string, string> linkMap = new Dictionary<string, string>();

            foreach (ControllerActionDescriptor action in this.ActionDescriptors)
            {
                string actionHttpMethod = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .SelectMany(x => x.HttpMethods)
                            .Distinct()
                            .First();

                bool currentActionIsElementGetter = action.EndpointMetadata.Any(x => x is ElementGetterAttribute);

                if (actionHttpMethod.ToUpper() == "GET"
                    && (currentActionIsElementGetter
                        || CurrentAction.DisplayName != action.ActionName))
                {
                    var doormanAttr = (HateoasActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is HateoasActionAttribute);

                    if (doormanAttr.ResourceCode == resourceCode
                        && doormanAttr.ActionScope == scope)
                    {
                        var doormanElementGetterAttr = action.EndpointMetadata.FirstOrDefault(x => x is ElementGetterAttribute);
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

        protected string ReplaceUrlTemplate(string urlTemplate, object element)
        {
            string result = urlTemplate;

            if (element != null)
            {
                // Este regex trae el texto ENTRE llaves {} pero sin las llaves
                Regex regex = new Regex(@"(?<={)(.*?)(?=})");
                var match = regex.Match(result);

                if (match.Success)
                {
                    var elementPropList = element.GetType().GetProperties();

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

        protected Dictionary<string, HateoasAction> GetHateoasActions(object element, string resourceCode, ActionScope scope)
        {
            Dictionary<string, HateoasAction> actionMap = new Dictionary<string, HateoasAction>();

            foreach (ControllerActionDescriptor action in this.ActionDescriptors)
            {
                string actionHttpMethod = action
                            .EndpointMetadata
                            .OfType<HttpMethodAttribute>()
                            .SelectMany(x => x.HttpMethods)
                            .Distinct()
                            .First();

                bool currentActionIsElementGetter = action.EndpointMetadata.Any(x => x is ElementGetterAttribute);

                if (actionHttpMethod.ToUpper() != "GET"
                    && (currentActionIsElementGetter
                        || CurrentAction.DisplayName != action.ActionName))
                {
                    var doormanAttr = (HateoasActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is HateoasActionAttribute);

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
                        newAction.Href = "/" + httpMethodAttr.Template;

                        if (element != null)
                            newAction.Href = ReplaceUrlTemplate(newAction.Href, element);

                        actionMap.Add(actionName, newAction);
                    }
                }
            }

            return actionMap;
        }
    }
}