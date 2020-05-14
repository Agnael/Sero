using Sero.Core;
using Sero.Gatekeeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class Role : IApiResource
    {
        public string ApiResourceCode => GtkResourceCodes.Roles;
        public string OwnerId { get; set; }

        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<Permission> Permissions { get; set; }

        public Role()
        {
            this.Permissions = new List<Permission>();
        }

        public Role (
            string code, 
            string name, 
            string description, 
            params Permission[] permissions)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentNullException(nameof(code));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            this.Code = code;
            this.DisplayName = name;
            this.Description = description;
            this.Permissions = permissions.ToList();
        }
    }
}
