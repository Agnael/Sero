using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Controller
{
    public class RoleUpdateForm
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public List<Permission> Permissions { get; set; }

        public RoleUpdateForm()
        {
            this.Permissions = new List<Permission>();
        }

        public RoleUpdateForm(string displayName, string description, List<Permission> permissions)
        {
            this.DisplayName = displayName;
            this.Description = description;
            this.Permissions = permissions;
        }
    }
}
