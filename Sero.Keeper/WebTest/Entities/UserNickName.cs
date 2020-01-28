using Sero.Keeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTest.Entities
{
    public class UserNickName : Value
    {
        public int IdUser { get; set; }
        public User User { get; set; }
    }
}
