using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Sero.Core
{
    public abstract class BaseHateoasController : ControllerBase
    {
        protected BaseHateoasController()
        {

        }

        protected ObjectResult ValidationError()
        {
            return BadRequest(this.ModelState);
        }

        public ObjectResult NotFound<TElement>(
            ICollectionFilter usedFilter,
            int totalElementsExisting)
            where TElement : IApiResource
        {
            var result = this.Collection<TElement>(usedFilter, totalElementsExisting, new List<IApiResource>());
            return StatusCode(StatusCodes.Status404NotFound, result);
        }

        public ObjectResult NotFound<TElement>(
            ICollectionFilter usedFilter,
            IPage<IApiResource> resultsPage)
            where TElement : IApiResource
        {
            var result = this.Collection<TElement>(usedFilter, resultsPage.Total, new List<IApiResource>());
            return StatusCode(StatusCodes.Status404NotFound, result);
        }

        protected ObjectResult Collection<TElement>(
            ICollectionFilter usedFilter,
            int totalElementsExisting,
            IEnumerable<IApiResource> elementsToReturn)
            where TElement : IApiResource
        {
            var filterOverview = usedFilter.GetOverview();
            var view = new CollectionView<TElement>(filterOverview, totalElementsExisting, elementsToReturn);
            return new ObjectResult(view);
        }

        protected ObjectResult Collection<TElement>(
            ICollectionFilter usedFilter,
            IPage<IApiResource> resultsPage)
            where TElement : IApiResource
        {
            var filterOverview = usedFilter.GetOverview();
            var view = new CollectionView<TElement>(filterOverview, resultsPage.Total, resultsPage.Items);
            return new ObjectResult(view);
        }

        protected ObjectResult Element<TElement>(IApiResource elementToReturn)
            where TElement : IApiResource
        {
            var view = new ElementView<TElement>(elementToReturn);
            return new ObjectResult(view);
        }
    }
}
