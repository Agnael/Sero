using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Mvc.ViewModels
{
    public class ValidationErrorView
    {
        public Dictionary<string, string[]> ErrorMap { get; set; }
    }
}
