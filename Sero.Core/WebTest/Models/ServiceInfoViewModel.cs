using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Models
{
    public class ServiceInfoViewModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        public ServiceInfoViewModel()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}
