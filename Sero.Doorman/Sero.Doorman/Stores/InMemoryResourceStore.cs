using Sero.Core;
using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public class InMemoryResourceStore : IResourceStore
    {
        public readonly IList<Resource> Resources;

        public InMemoryResourceStore(IList<Resource> resourceList)
        {
            Resources = resourceList;
        }

        public async Task<Page<Resource>> Get(ResourceFilter filter)
        {
            Func<Resource, string> orderByPredicate = null;
            
            // Construye el predicate de ordenamiento en función del nombre de campo, es horrendo para este caso puntual pero
            // haciendolo así se tiene la flexibilidad de poder usar otro tipo diferente al modelo Resource.
            if (filter.SortBy == ResourceSorting.Code)
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == ResourceSorting.Category)
                orderByPredicate = x => x.Category;

            IEnumerable<Resource> query = Resources;

            if (filter.OrderBy == Order.Desc)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.FreeText))
                query = query.Where(x => x.Code.ToLower().Contains(filter.FreeText.ToLower())
                                        || x.Category.ToLower().Contains(filter.FreeText.ToLower()));

            int count = query.Count();

            var list = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new Page<Resource>(count, list);
        }

        public async Task<Resource> Get(string resourceCode)
        {
            var result = Resources.FirstOrDefault(x => x.Code == resourceCode);
            return result;
        }

        public async Task<IEnumerable<string>> GetAllCodes()
        {
            var allCodes = Resources.Select(x => x.Code);
            return allCodes;
        }

        public async Task<bool> IsExisting(string resourceCode)
        {
            var result = Resources.Any(x => x.Code == resourceCode);
            return result;
        }

        public async Task Update(Resource resource)
        {
            var fetched = Resources.FirstOrDefault(x => x.Code == resource.Code);
            resource.Category = resource.Category;
            resource.Description = resource.Description;
        }
    }
}
