using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public interface IResourceStore
    {
        Task<IPage<Resource>> Get(ResourceFilter filter);
        Task<Resource> Get(string resourceCode);
        Task<IEnumerable<string>> GetAllCodes();

        Task Create(Resource resource);
        Task Create(string category, string code, string description);
        Task Update(Resource resource);
        Task<bool> IsExisting(string resourceCode);
    }
}
