using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class ResourcesFilter : CollectionFilter
    {
        public ResourcesFilter()
        {
        }

        public ResourcesFilter(CollectionFilter filter) : base(filter)
        {
        }

        public ResourcesFilter(string textSearch, int page, int pageSize, string sortBy, string orderBy) 
            : base(textSearch, page, pageSize, sortBy, orderBy)
        {
        }

        public override CollectionFilter Copy()
        {
            return new ResourcesFilter(this);
        }

        public override string GetDefaultSortByValue()
        {
            return nameof(Resource.Code);
        }
    }
}
