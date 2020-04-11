using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests.Controllers.Resources
{
    public class ResourcesControllerFixture : IDisposable
    {
        protected readonly ResourcesController _sut;

        protected readonly ResourcesControllerBuilder _sutBuilder;
        protected readonly InMemoryResourceStore _resourceStore;

        protected readonly ResourceComparer _resourceComparer;

        public ResourcesControllerFixture()
        {
            _sutBuilder = new ResourcesControllerBuilder();
            _resourceComparer = new ResourceComparer();

            _resourceStore = 
                new ResourceStoreBuilder()
                .WithDefaultResources()
                .Build();

            _sut = new ResourcesController(_resourceStore);
        }

        public void Dispose()
        {
            
        }
    }
}
