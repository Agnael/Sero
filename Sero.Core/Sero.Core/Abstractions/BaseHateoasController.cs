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
               
        protected ObjectResult Collection<TElement>(
            ICollectionFilter usedFilter,
            int totalElementsExisting,
            IEnumerable<object> elementsToReturn)
            where TElement : Element
        {
            var filterOverview = usedFilter.GetOverview();
            var view = new CollectionView<TElement>(filterOverview, totalElementsExisting, elementsToReturn);
            return new ObjectResult(view);
        }

        protected ObjectResult Element<TElement>(object elementToReturn)
            where TElement : Element
        {
            var view = new ElementView<TElement>(elementToReturn);
            return new ObjectResult(view);
        }
    }
}
