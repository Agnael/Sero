using Sero.Doorman;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class Resource
    {
        public string Code { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public Resource(string code)
        {
            this.Category = "uncategorized";
            this.Code = code;
        }

        public Resource(string category, string code)
        {
            this.Category = category;
            this.Code = code;
        }

        public Resource(string category, string code, string description)
        {
            this.Category = category;
            this.Code = code;
            this.Description = description;
        }
    }
}
