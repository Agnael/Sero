using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Doorman
{
    public class AuthorizationContext
    {
        public Session Session { get; private set; }
        public IList<Permission> Permissions { get; private set; }

        public AuthorizationContext()
        {
            this.Permissions = new List<Permission>();
        }

        public void SetSession(Session session)
        {
            this.Session = session;
        }

        public void TryAddPermission(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var existing = this.Permissions.FirstOrDefault(x => x.ResourceCode == permission.ResourceCode);

            if (existing != null)
            {
                if (permission.Level > existing.Level)
                {
                    this.Permissions.Remove(existing);
                    this.Permissions.Add(permission);
                }
            }
            else
            {
                this.Permissions.Add(permission);
            }
        }

        public void TryAddRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));

            if(role.Permissions.Count > 0)
            {
                foreach(var permission in role.Permissions)
                    this.TryAddPermission(permission);
            }
        }
    }
}
