using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Nicknames { get; set; }

        public User()
        {
            Nicknames = new List<string>();
        }
    }
}
