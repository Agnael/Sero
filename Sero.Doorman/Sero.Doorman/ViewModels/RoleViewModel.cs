using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.ViewModels
{
    public class RoleViewModel
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }

        public RoleViewModel(Role role)
        {
            this.Code = role.Code;
            this.DisplayName = role.DisplayName;
        }
    }
}
