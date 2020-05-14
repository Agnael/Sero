using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests.Controllers.Credentials
{
    public class CredentialsControllerFixture : IDisposable
    {
        protected readonly CredentialsController _sut;

        protected readonly CredentialsControllerBuilder _sutBuilder;

        protected readonly InMemoryCredentialStore _credentialStore;
        protected readonly InMemoryRoleStore _roleStore;
        protected readonly InMemoryResourceStore _resourceStore;

        protected readonly CredentialVmComparer _credentialVmComparer;

        public CredentialsControllerFixture()
        {
            _sutBuilder = new CredentialsControllerBuilder();

            _credentialVmComparer = new CredentialVmComparer(new RoleVmComparer());

            _resourceStore = 
                new ResourceStoreBuilder()
                .WithDefaultResources()
                .Build();

            _roleStore = 
                new RoleStoreBuilder(_resourceStore)
                .WithDefaultRoles()
                .Build();

            _credentialStore = 
                new CredentialStoreBuilder(_roleStore)
                .WithDefaultCredentials()
                .Build();

            _sut = new CredentialsController(_roleStore, _resourceStore, _credentialStore);
        }

        public void Dispose()
        {
            
        }
    }
}
