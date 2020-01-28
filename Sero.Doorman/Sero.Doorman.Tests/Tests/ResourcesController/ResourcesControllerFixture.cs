using Sero.Doorman.Controller;
using Sero.Doorman.Tests.Comparers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests.Controllers.Resources
{
    public class ResourcesControllerFixture : IDisposable
    {
        protected readonly ResourcesController _defaultSut;

        protected readonly ResourcesControllerBuilder _sutBuilder;
        protected readonly ResourceStoreBuilder _resourceStoreBuilder;

        protected readonly ResourceComparer _resourceComparer;

        public ResourcesControllerFixture()
        {
            _sutBuilder = new ResourcesControllerBuilder();
            _resourceComparer = new ResourceComparer();
            _resourceStoreBuilder = new ResourceStoreBuilder()
                .WithDefaultResources();

            _defaultSut = _sutBuilder
                .WithResourceStore(_resourceStoreBuilder.Build())
                .Build();
        }

        public void Dispose()
        {
            
        }
    }
}
