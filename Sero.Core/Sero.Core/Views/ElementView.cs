using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public abstract class ElementView : IResultView
    {
        public abstract string ResourceCode { get; }
        public readonly IApiResource ViewModel;

        public ElementView(IApiResource viewModel)
        {
            this.ViewModel = viewModel;
        }
    }

    public class ElementView<TElement> : ElementView
        where TElement : IApiResource
    {
        public override string ResourceCode { 
            get
            {
                string resourceCode = Activator.CreateInstance<TElement>().ApiResourceCode;
                return resourceCode;
            } 
        }

        public ElementView(IApiResource viewModel)
            : base(viewModel)
        {

        }
    }
}
