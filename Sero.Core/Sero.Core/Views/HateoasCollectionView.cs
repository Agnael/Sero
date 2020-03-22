using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class HateoasCollectionView : HateoasObject
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string OrderedBy { get; set; }
        public string SortedBy { get; set; }

        public HateoasCollectionView(
            CollectionFilter filter, 
            int totalElementsAvailable,
            Dictionary<string, string> collectionLinks,
            Dictionary<string, HateoasAction> collectionActions,
            IEnumerable<object> embeddedList,
            HateoasLabeledLink parentLink = null)
        {
            this.Page = filter.Page;
            this.PageSize = filter.PageSize;
            this.Total = totalElementsAvailable;
            this.TotalPages = (int)Math.Ceiling((double)Total / PageSize);
            this.OrderedBy = filter.OrderBy;
            this.SortedBy = filter.SortBy;

            this._links = collectionLinks;
            this._actions = collectionActions;
            this._embedded = embeddedList;
            this._parent = parentLink;
        }
    }
}
