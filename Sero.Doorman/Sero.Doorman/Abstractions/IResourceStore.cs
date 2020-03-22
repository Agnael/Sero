using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface IResourceStore
    {
        Task<Page<Resource>> Get(ResourcesFilter filter);

        Task<Resource> Get(string resourceCode);
        Task Update(Resource resource);
        Task<bool> IsUnique(string resourceCode);
    }
}
