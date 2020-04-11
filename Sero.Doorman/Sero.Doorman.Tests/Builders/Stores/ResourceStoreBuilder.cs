using Moq;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Tests
{
    public class ResourceStoreBuilder
    {
        public readonly List<Resource> ResourceList;

        public ResourceStoreBuilder()
        {
            ResourceList = new List<Resource>();
        }

        public ResourceStoreBuilder WithDefaultResources()
        {
            ResourceList.Add(ResourceData.Resource_01);
            ResourceList.Add(ResourceData.Resource_02);
            ResourceList.Add(ResourceData.Resource_03);
            ResourceList.Add(ResourceData.Resource_04);
            ResourceList.Add(ResourceData.Resource_05);
            ResourceList.Add(ResourceData.Resource_06);
            ResourceList.Add(ResourceData.Resource_07);
            ResourceList.Add(ResourceData.Resource_08);
            ResourceList.Add(ResourceData.Resource_09);
            ResourceList.Add(ResourceData.Resource_10);

            return this;
        }

        public ResourceStoreBuilder WithResource(string category, string code, string description)
        {
            ResourceList.Add(new Resource(category, code, description));
            return this;
        }

        public InMemoryResourceStore Build()
        {
            return new InMemoryResourceStore(ResourceList);
        }
    }
}
