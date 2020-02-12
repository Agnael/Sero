using Moq;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests
{
    public class RolesControllerBuilder
    {
        public IRoleStore RoleStore { get; private set; }
        public IResourceStore ResourceStore { get; private set; }

        public RolesControllerBuilder()
        {
            RoleStore = new Mock<IRoleStore>().Object;
            ResourceStore = new Mock<IResourceStore>().Object;
        }

        public RolesControllerBuilder WithRoleStore(IRoleStore store)
        {
            this. RoleStore = store;
            return this;
        }

        public RolesControllerBuilder WithResourceStore(IResourceStore store)
        {
            this.ResourceStore = store;
            return this;
        }

        public RolesController Build()
        {
            // TODO: AGREGUÉ ESTE NULL PARA PROBAR ALGO DE HATEOAS PERO POR DIOS PENSÁ BIEN QUÉ METER ACA
            return new RolesController(RoleStore, ResourceStore);
        }
    }
}
