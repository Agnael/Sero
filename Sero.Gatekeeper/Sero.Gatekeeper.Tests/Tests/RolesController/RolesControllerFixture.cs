using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests.Controllers.Roles
{
    public class RolesControllerFixture : IDisposable
    {
        protected readonly RolesController _sut;

        protected readonly RolesControllerBuilder _sutBuilder;
        protected readonly InMemoryRoleStore _roleStore;
        protected readonly InMemoryResourceStore _resourceStore;

        protected readonly PermissionComparer _permissionComparer;
        protected readonly RoleComparer _roleComparer;

        public RolesControllerFixture()
        {
            _sutBuilder = new RolesControllerBuilder();

            _resourceStore = 
                new ResourceStoreBuilder()
                .WithDefaultResources()
                .Build();

            _roleStore = 
                new RoleStoreBuilder(_resourceStore)
                .WithDefaultRoles()
                .Build();

            _sut = new RolesController(_roleStore, _resourceStore);

            _permissionComparer = new PermissionComparer();
            _roleComparer = new RoleComparer(_permissionComparer);
        }

        public void Dispose()
        {
            
        }
    }
}
