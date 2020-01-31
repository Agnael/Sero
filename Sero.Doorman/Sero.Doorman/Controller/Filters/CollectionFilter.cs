using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public abstract class CollectionFilter
    {
        public string TextSearch { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }

        public string SortBy { get; set; }
        public string OrderBy { get; set; }

        public abstract string GetDefaultSortingField();

        public CollectionFilter()
        {
            this.TextSearch = null;
            this.Page = 1;
            this.PageSize = 10;
            this.SortBy = GetDefaultSortingField();
            this.OrderBy = Order.ASC;
        }

        public CollectionFilter(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            this.TextSearch = textSearch;
            this.Page = page;
            this.PageSize = pageSize;
            this.SortBy = sortBy;
            this.OrderBy = orderBy;
        }
    }
}
