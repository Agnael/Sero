﻿using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class RoleVM
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }

        public RoleVM(Role role)
        {
            this.Code = role.Code;
            this.DisplayName = role.DisplayName;
        }
    }
}
