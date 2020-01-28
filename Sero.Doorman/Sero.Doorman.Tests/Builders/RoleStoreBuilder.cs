using Moq;
using Sero.Doorman.Controller;
using Sero.Doorman.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests
{
    public class RoleStoreBuilder
    {
        public readonly List<Role> RoleList;

        public RoleStoreBuilder()
        {
            RoleList = new List<Role>();
        }

        public RoleStoreBuilder WithDefaultRoles()
        {
            RoleList.Add(RoleData.Role_01_Admin);
            RoleList.Add(RoleData.Role_02_User);

            return this;
        }

        public RoleStoreBuilder WithRole(string code, string name, string description, Permission[] permissions)
        {
            RoleList.Add(new Role(code, name, description, permissions));
            return this;
        }

        public IRoleStore Build()
        {
            return new InMemoryRoleStore(RoleList);
        }
    }
}
