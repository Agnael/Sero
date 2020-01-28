using Sero.Keeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }

        public ICollection<UserNickName> NickNames { get; set; }
    }
}
