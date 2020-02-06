using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman.Stores
{
    public class InMemoryResourceStore : IResourceStore
    {
        public readonly IList<Resource> Resources;

        public InMemoryResourceStore(IList<Resource> resourceList)
        {
            Resources = resourceList;
        }

        public async Task<int> CountAsync(ResourcesFilter filter)
        {
            IEnumerable<Resource> query = Resources;
            Func<Resource, string> orderByPredicate = null;

            // Construye el predicate de ordenamiento en función del nombre de campo, es horrendo para este caso puntual pero
            // haciendolo así se tiene la flexibilidad de poder usar otro tipo diferente al modelo Resource.
            if (filter.SortBy.ToLower() == nameof(Resource.Code).ToLower())
                orderByPredicate = x => x.Code;
            else if (filter.SortBy.ToLower() == nameof(Resource.Category).ToLower())
                orderByPredicate = x => x.Category;

            if (filter.OrderBy.ToLower() == Order.DESC.ToLower())
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
                query = query.Where(x => x.Code.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.Category.ToLower().Contains(filter.TextSearch.ToLower()));

            var result = query.Count();

            return result;
        }

        public async Task<ICollection<Resource>> FetchAsync(ResourcesFilter filter)
        {
            IEnumerable<Resource> query = Resources;
            Func<Resource, string> orderByPredicate = null;
            
            // Construye el predicate de ordenamiento en función del nombre de campo, es horrendo para este caso puntual pero
            // haciendolo así se tiene la flexibilidad de poder usar otro tipo diferente al modelo Resource.
            if (filter.SortBy == nameof(Resource.Code))
                orderByPredicate = x => x.Code;
            else if (filter.SortBy == nameof(Resource.Category))
                orderByPredicate = x => x.Category;

            if (filter.OrderBy == Order.DESC)
                query = query.OrderByDescending(orderByPredicate);
            else
                query = query.OrderBy(orderByPredicate);

            if (!string.IsNullOrEmpty(filter.TextSearch))
                query = query.Where(x => x.Code.ToLower().Contains(filter.TextSearch.ToLower())
                                        || x.Category.ToLower().Contains(filter.TextSearch.ToLower()));

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return result;
        }

        public async Task<Resource> FetchAsync(string resourceCode)
        {
            var result = Resources.FirstOrDefault(x => x.Code == resourceCode);
            return result;
        }

        public async Task<bool> IsExistingAsync(string resourceCode)
        {
            var result = Resources.Any(x => x.Code == resourceCode);
            return result;
        }

        public async Task UpdateAsync(Resource resource)
        {
            var fetched = Resources.FirstOrDefault(x => x.Code == resource.Code);
            resource.Category = resource.Category;
            resource.Description = resource.Description;
        }
    }
}
