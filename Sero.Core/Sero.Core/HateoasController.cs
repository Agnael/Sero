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

        protected ObjectResult Collection(
            CollectionFilter usedFilter,
            int totalElementsExisting,
            IEnumerable<object> elementsToReturn)
        {
            var view = new CollectionResult(usedFilter, totalElementsExisting, elementsToReturn);
            return new ObjectResult(view);
        }

        protected ObjectResult Element(object elementToReturn)
        {
            return new ObjectResult(elementToReturn);
        }

        protected ObjectResult ChildElement(object parent, object child)
        {
            var result = new ChildElementResult(parent, child);
            return new ObjectResult(result);
        }
    }
}
