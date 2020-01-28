using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface IResourceStore
    {
        Task<ICollection<Resource>> FetchAsync(ResourcesFilter filter);
        Task<Resource> FetchAsync(string resourceCode);
        Task UpdateAsync(Resource resource);
        Task<bool> IsExistingAsync(string resourceCode);
    }
}
