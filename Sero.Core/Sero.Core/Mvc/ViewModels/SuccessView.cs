using Sero.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Mvc
{
    public class SuccessView
    {
        public Dictionary<string, object> _links { get; set; }
        public object _embeded { get; set; }

        public SuccessView()
        {
            _links = new Dictionary<string, object>();
        }
    }
}
