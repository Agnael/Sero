using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core.Mvc.Models
{
    public class ResourceLink
    {
        public string UrlTemplate { get; set; }
        public string Verb { get; set; }
        public Dictionary<string, string> ParameterMap { get; set; }

        public ResourceLink()
        {
            this.ParameterMap = new Dictionary<string, string>();
        }
    }
}
