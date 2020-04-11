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
        public readonly object ViewModel;

        public ElementView(object viewModel)
        {
            this.ViewModel = viewModel;
        }
    }

    public class ElementView<TElement> : ElementView
        where TElement : Element
    {
        public override string ResourceCode { 
            get
            {
                string resourceCode = Activator.CreateInstance<TElement>().GetAppResourceCode();
                return resourceCode;
            } 
        }

        public ElementView(object viewModel)
            : base(viewModel)
        {

        }
    }
}
