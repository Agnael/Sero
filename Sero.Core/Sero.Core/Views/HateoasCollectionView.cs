using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core
{
    public class HateoasCollectionView : HateoasObject
    {
        public int Total { get; set; }
        public int TotalPages { get; set; }

        public Dictionary<string, object> _filter { get; set; }

        public HateoasCollectionView(
            int totalElementsAvailable,
            int totalPages,
            Dictionary<string, object> filtersUsed,
            Dictionary<string, string> collectionLinks,
            Dictionary<string, HateoasAction> collectionActions,
            IEnumerable<object> embeddedList,
            HateoasLabeledLink parentLink = null)
            : base(collectionLinks, collectionActions, embeddedList, parentLink)
        {
            this.Total = totalElementsAvailable;
            this.TotalPages = totalPages;

            _filter = filtersUsed;
        }
    }
}
