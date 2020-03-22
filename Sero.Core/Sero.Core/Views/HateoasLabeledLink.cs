using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class HateoasLabeledLink
    {
        public string Title { get; set; }
        public string Href { get; set; }

        public HateoasLabeledLink(string title, string url)
        {
            this.Title = title;
            this.Href = url;
        }
    }
}
