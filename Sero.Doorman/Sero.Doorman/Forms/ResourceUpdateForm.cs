using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class ResourceUpdateForm
    {
        public string Category { get; set; }
        public string Description { get; set; }

        public ResourceUpdateForm()
        {

        }

        public ResourceUpdateForm(string category, string description)
        {
            this.Category = category;
            this.Description = description;
        }
    }
}
