using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public class HateoasController : ControllerBase
    {
        public HateoasController()
        {

        }

        protected ObjectResult ValidationError()
        {
            return BadRequest(this.ModelState);
        }
               
        protected ObjectResult Collection<TElement>(
            CollectionFilter usedFilter,
            int totalElementsExisting,
            IEnumerable<object> elementsToReturn)
            where TElement : Element
        {
            var view = new CollectionView<TElement>(usedFilter, totalElementsExisting, elementsToReturn);
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
