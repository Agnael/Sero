using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class RequestUtils
    {
        public bool IsInitialized { get; private set; }

        public ActionDescriptor CurrentAction { get; private set; }

        private IDictionary<string, object> _currentActionArguments;
        public IDictionary<string, object> CurrentActionArguments 
        {
            get
            {
                if (!IsInitialized)
                    throw new UninitializedDoormanActionArgumentHolderService();

                return _currentActionArguments;
            }
            private set
            {
                _currentActionArguments = value;
            } 
        }

        public readonly IReadOnlyList<ActionDescriptor> ActionDescriptors;

        public RequestUtils(IActionDescriptorCollectionProvider actionDescriptorProvider)
        {
            this.ActionDescriptors = actionDescriptorProvider.ActionDescriptors.Items;
        }

        public void Initialize(ActionExecutingContext context)
        {
            CurrentActionArguments = context.ActionArguments;
            context.ActionDescriptor = context.ActionDescriptor;

            IsInitialized = true;
        }
    }
}
