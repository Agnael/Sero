using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Models
{
    public class Menu
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category[] Categories { get; set; }
        public Resource[] Resources { get; set; }
    }
}
