using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class RolesFilter : CollectionFilter
    {
        public RolesFilter()
        {
        }

        public RolesFilter(CollectionFilter filter) : base(filter)
        {
        }

        public RolesFilter(string textSearch, ushort page, ushort pageSize, string sortBy, string orderBy) : base(textSearch, page, pageSize, sortBy, orderBy)
        {
        }

        public override CollectionFilter Copy()
        {
            return new RolesFilter(this);
        }

        public override string GetDefaultSortByValue()
        {
            return nameof(Role.Code);
        }
    }
}
