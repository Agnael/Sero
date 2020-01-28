using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Sero.Core.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Sero.Core.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Sero.Core.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Web;
using Sero.Core;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace Sero.Doorman.Controller
{
    public abstract class ResourceController<TResource> : ControllerBase
    {
        public readonly IRequestInfoService RequestInfoService;
        public readonly IReadOnlyList<ActionDescriptor> ActionDescriptors;

        protected DoormanActionAttribute _doormanActionAttr;
        protected DoormanActionAttribute DoormanActionAttr
        {
            get
            {
                if(_doormanActionAttr == null)
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

        public ResourceController(IRequestInfoService reqInfoService, 
            IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            this.RequestInfoService = reqInfoService;
            this.ActionDescriptors = actionDescriptorProvider.ActionDescriptors.Items;
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
            var view = new SuccessView();
            view._links = GetMetaLinks(this.DoormanActionAttr.ResourceCode, this.DoormanActionAttr.ActionScope);
            view._links.Add("_self", GetSelfMetaLink());

            view._embeded = resource;

            return new ObjectResult(view);
        }

        protected async Task<IActionResult> SuccessAsync(IEnumerable<TResource> resourceList)
        {
            var view = new SuccessView();
            view._links = GetMetaLinks(this.DoormanActionAttr.ResourceCode, this.DoormanActionAttr.ActionScope);
            view._links.Add("_self", GetSelfMetaLink());

            List<SuccessView> embeddedList = new List<SuccessView>();

            foreach(TResource resource in resourceList)
            {
                SuccessView embeddedResource = new SuccessView();
                embeddedResource._links = GetMetaLinks(this.DoormanActionAttr.ResourceCode, ActionScope.Element);
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

                foreach(string qsKey in qsNameValues.AllKeys)
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

            foreach (ControllerActionDescriptor action in this.ActionDescriptors)
            {
                var doormanAttr = (DoormanActionAttribute)action.EndpointMetadata.FirstOrDefault(x => x is DoormanActionAttribute);

                if (doormanAttr != null
                    && doormanAttr.ResourceCode == resourceCode
                    && doormanAttr.ActionScope == scope
                    && doormanAttr.ActionCode != this.DoormanActionAttr.ActionCode)
                {
                    ResourceLink newLink = new ResourceLink();

                    //var routeAttr = (RouteAttribute)action.EndpointMetadata.FirstOrDefault(x => x is RouteAttribute);
                    //if (routeAttr != null)
                    //    newLink.UrlTemplate = routeAttr.Template;

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

                    //if(newLink.Verb == "GET"
                    //    && action.Parameters.Count == 1
                    //    && !action.Parameters.First().ParameterType.IsPrimitive)
                    //{
                    //    var httpGetActionProps = action.Parameters.First().ParameterType.GetProperties();

                    //    foreach (PropertyInfo prop in httpGetActionProps)
                    //        newLink.ParameterMap.Add(prop.Name, prop.PropertyType.Name);
                    //}
                    //else
                    //{
                        foreach (ParameterDescriptor param in action.Parameters)
                            newLink.ParameterMap.Add(param.Name, param.ParameterType.Name);
                    //}

                    linkMap.Add(doormanAttr.ActionCode, newLink);
                }
            }

            return linkMap;
        }
    }
}
