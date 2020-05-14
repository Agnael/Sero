using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage.Stores.InMemory
{
    public class InMemoryUserStore : IUserStore
    {
        public readonly IList<User> Users;

        public InMemoryUserStore(IList<User> userList)
        {
            Users = userList;
        }

        public async Task<User> Get(string okAuthCredentialId)
        {
            var user = Users.FirstOrDefault(x => x.OkAuthCredentialId == okAuthCredentialId);
            return user;
        }

        public async Task<IPage<Role>> GetRoles(string okAuthCredentialId, RoleFilter filter)
        {
            var user = await this.Get(okAuthCredentialId);

            if (user == null)
                return new Page<Role>();

            var roleStore = new InMemoryRoleStore(user.Roles);

            var roles = await roleStore.Get(filter);
            return roles;
        }

        public async Task<IList<Role>> GetRoles(string okAuthCredentialId)
        {
            var result =
                Users
                .FirstOrDefault(x =>
                    x.OkAuthCredentialId == okAuthCredentialId);

            return result.Roles;
        }
    }
}
