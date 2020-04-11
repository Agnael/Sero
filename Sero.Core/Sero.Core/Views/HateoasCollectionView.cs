using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core
{
    public class HateoasCollectionView : HateoasObject
    {
        public Dictionary<string, string> _usedFilter { get; set; }

        public int Total { get; set; }
        public int TotalPages { get; set; }

        public HateoasCollectionView(
            int totalElementsAvailable,
            int totalPages,
            Dictionary<string, string> collectionLinks,
            Dictionary<string, HateoasAction> collectionActions,
            IEnumerable<object> embeddedList,
            HateoasLabeledLink parentLink = null)
        {
            this.Total = totalElementsAvailable;
            this.TotalPages = totalPages;

            this._links = collectionLinks;
            this._actions = collectionActions;
            this._embedded = embeddedList;
            this._parent = parentLink;
        }
    }
}
