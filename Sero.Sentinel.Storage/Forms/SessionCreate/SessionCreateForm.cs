using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public class SessionCreateForm
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

        public bool IsRememberLogin { get; set; }
        public string ReturnUrl { get; set; }
    }
}
