using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface IResourceStore
    {
        Task<Page<Resource>> Get(ResourceFilter filter);
        Task<Resource> Get(string resourceCode);
        Task<IEnumerable<string>> GetAllCodes();

        Task Update(Resource resource);
        Task<bool> IsExisting(string resourceCode);
    }
}
