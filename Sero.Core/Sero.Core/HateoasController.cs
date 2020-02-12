using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public class HateoasController<TResource> : ControllerBase where TResource : class
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
            IEnumerable<TResource> elementsToReturn)
        {
            var view = new CollectionResult(usedFilter, totalElementsExisting, elementsToReturn);
            return new ObjectResult(view);
        }

        protected ObjectResult Element(TResource elementToReturn)
        {
            return new ObjectResult(elementToReturn);
        }

        //protected ObjectResult Created(TResource elementToReturn)
        //{
        //    var result = new ObjectResult(elementToReturn);
        //    result.StatusCode = StatusCodes.Status201Created;

        //    return result;
        //}

        //protected ObjectResult Accepted(TResource elementToReturn)
        //{
        //    var result = new ObjectResult(elementToReturn);
        //    result.StatusCode = StatusCodes.Status202Accepted;
            
        //    return result;
        //}
    }
}
