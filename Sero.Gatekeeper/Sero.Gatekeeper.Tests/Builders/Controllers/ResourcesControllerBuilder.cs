﻿using Moq;
using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Tests
{
    public class ResourcesControllerBuilder
    {
        public IResourceStore ResourceStore { get; private set; }

        public ResourcesControllerBuilder()
        {
            ResourceStore = new Mock<IResourceStore>().Object;
        }

        public ResourcesControllerBuilder WithResourceStore(IResourceStore store)
        {
            ResourceStore = store;
            return this;
        }

        public ResourcesController Build()
        {
            // TODO: OLEG CAMBIÁ ESE NULL QUE ESTOY PASANDO PARA CREAR EL RESOURCE CONTROLLER DEL TEST, ES SOLO PARA QUE BUILDEE LA SOLUTION
            return new ResourcesController(ResourceStore);
        }
    }
}
