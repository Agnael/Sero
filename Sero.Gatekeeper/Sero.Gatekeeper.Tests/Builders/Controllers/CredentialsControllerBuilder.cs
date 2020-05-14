using Moq;
using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public class CredentialsControllerBuilder
    {
        public IRoleStore RoleStore { get; private set; }
        public IResourceStore ResourceStore { get; private set; }
        public ICredentialStore CredentialStore { get; private set; }

        public CredentialsControllerBuilder()
        {
            RoleStore = new Mock<IRoleStore>().Object;
            ResourceStore = new Mock<IResourceStore>().Object;
            CredentialStore = new Mock<ICredentialStore>().Object;
        }

        public CredentialsControllerBuilder WithRoleStore(IRoleStore store)
        {
            this. RoleStore = store;
            return this;
        }

        public CredentialsControllerBuilder WithResourceStore(IResourceStore store)
        {
            this.ResourceStore = store;
            return this;
        }

        public CredentialsControllerBuilder WithCredentialStore(ICredentialStore store)
        {
            this.CredentialStore = store;
            return this;
        }

        public CredentialsController Build()
        {
            return new CredentialsController(RoleStore, ResourceStore, CredentialStore);
        }
    }
}
