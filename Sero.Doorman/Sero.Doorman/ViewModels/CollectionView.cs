using Sero.Doorman.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class CollectionView : SuccessView
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string OrderedBy { get; set; }
        public string SortedBy { get; set; }

        public CollectionView(CollectionFilter filter, int totalElementsAvailable)
        {
            this.Page = filter.Page;
            this.PageSize = filter.PageSize;
            this.Total = totalElementsAvailable;
            this.TotalPages = (int)Math.Ceiling((double)Total / PageSize);
            this.OrderedBy = filter.OrderBy;
            this.SortedBy = filter.SortBy;
        }
    }
}
