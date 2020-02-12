using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public class CollectionResult
    {
        public readonly CollectionFilter UsedFilter;
        public readonly int TotalElementsExisting;
        public readonly IEnumerable<object> ElementsToReturn;

        public CollectionResult(
            CollectionFilter usedFilter, 
            int totalElementsExisting, 
            IEnumerable<object> elementsToReturn)
        {
            this.UsedFilter = usedFilter;
            this.TotalElementsExisting = totalElementsExisting;
            this.ElementsToReturn = elementsToReturn;
        }
    }
}
