using Sero.Doorman.Controller;
using Sero.Doorman.Tests.Comparers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests.Controllers.Roles
{
    public class RolesControllerFixture : IDisposable
    {
        protected readonly RolesController _defaultSut;

        protected readonly RolesControllerBuilder _sutBuilder;
        protected readonly RoleStoreBuilder _roleStoreBuilder;
        protected readonly ResourceStoreBuilder _resourceStoreBuilder;

        protected readonly PermissionComparer _permissionComparer;
        protected readonly RoleComparer _roleComparer;

        public RolesControllerFixture()
        {
            _sutBuilder = new RolesControllerBuilder();

            _roleStoreBuilder = new RoleStoreBuilder().WithDefaultRoles();
            _resourceStoreBuilder = new ResourceStoreBuilder().WithDefaultResources();

            _defaultSut = _sutBuilder
                .WithRoleStore(_roleStoreBuilder.Build())
                .WithResourceStore(_resourceStoreBuilder.Build())
                .Build();

            _permissionComparer = new PermissionComparer();
            _roleComparer = new RoleComparer(_permissionComparer);
        }

        public void Dispose()
        {
            
        }
    }
}
