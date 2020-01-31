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

        public ResourcesFilter(string textSearch, ushort page, ushort pageSize, string sortBy, string orderBy) : base(textSearch, page, pageSize, sortBy, orderBy)
        {
        }

        public override string GetDefaultSortingField()
        {
            return nameof(Resource.Code);
        }
    }
}
