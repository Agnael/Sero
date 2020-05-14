using Moq;
using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public class RoleStoreBuilder
    {
        public readonly IResourceStore ResourceStore;
        public readonly List<Role> RoleList;

        public RoleStoreBuilder(IResourceStore resourceStore)
        {
            ResourceStore = resourceStore;
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

        public InMemoryRoleStore Build()
        {
            var roleStore = new InMemoryRoleStore(RoleList);
            new RoleSeeder().Seed(ResourceStore, roleStore).Wait();
            return roleStore;
        }
    }
}
