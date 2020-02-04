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

        public CollectionFilter()
        {
            this.TextSearch = GetDefaultTextSearchValue();
            this.Page = GetDefaultPageValue();
            this.PageSize = GetDefaultPageSizeValue();
            this.SortBy = GetDefaultSortByValue();
            this.OrderBy = GetDefaultOrderByValue();
        }

        public CollectionFilter(string textSearch, int page, int pageSize, string sortBy, string orderBy)
        {
            this.TextSearch = textSearch;
            this.Page = page;
            this.PageSize = pageSize;
            this.SortBy = sortBy;
            this.OrderBy = orderBy;
        }

        public CollectionFilter(CollectionFilter filter)
        {
            this.TextSearch = filter.TextSearch;
            this.Page = filter.Page;
            this.PageSize = filter.PageSize;
            this.SortBy = filter.SortBy;
            this.OrderBy = filter.OrderBy;
        }

        public abstract string GetDefaultSortByValue();
        public abstract CollectionFilter Copy();

        public string GetDefaultTextSearchValue()
        {
            return null;
        }

        public int GetDefaultPageValue()
        {
            return 1;
        }

        public int GetDefaultPageSizeValue()
        {
            return 10;
        }

        public string GetDefaultOrderByValue()
        {
            return Order.ASC;
        }

        public bool IsDefaultTextSearch()
        {
            return this.TextSearch == GetDefaultTextSearchValue();
        }

        public bool IsDefaultPage()
        {
            return this.Page == GetDefaultPageValue();
        }

        public bool IsDefaultPageSize()
        {
            return this.PageSize == GetDefaultPageSizeValue();
        }

        public bool IsDefaultSortBy()
        {
            return this.SortBy.ToLower() == GetDefaultSortByValue().ToLower();
        }

        public bool IsDefaultOrderBy()
        {
            return this.OrderBy.ToLower() == GetDefaultOrderByValue().ToLower();
        }
    }
}
