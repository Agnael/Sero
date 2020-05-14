using Sero.Core;
using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Gatekeeper
{
    public class AuthorizationContext
    {
        public User User { get; private set; }

        /// <summary>
        ///     Permissions that are granted to the current context
        /// </summary>
        public IList<Permission> Permissions { get; private set; }

        public AuthorizationContext()
        {
            this.Permissions = new List<Permission>();
        }

        public void SetSession(User user)
        {
            this.User = user;
        }

        public void TryAddPermission(Permission permission)
        {
            if (permission == null) throw new ArgumentNullException(nameof(permission));

            var existing = this.Permissions.FirstOrDefault(x => x.ResourceCode == permission.ResourceCode);

            if (existing != null)
            {
                if (permission.LevelOnOwned > existing.LevelOnOwned)
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
