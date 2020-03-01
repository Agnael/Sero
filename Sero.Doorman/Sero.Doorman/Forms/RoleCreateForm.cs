using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class RoleCreateForm
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Permission[] Permissions { get; set; }

        public RoleCreateForm()
        {
            this.Permissions = new Permission[] { };
        }

        public RoleCreateForm(string code, string name, string description, Permission[] permissions)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
            this.Permissions = permissions;
        }
    }
}
