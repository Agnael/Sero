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

        public RolesFilter(string textSearch, ushort page, ushort pageSize, string sortBy, string orderBy) : base(textSearch, page, pageSize, sortBy, orderBy)
        {
        }

        public override string GetDefaultSortingField()
        {
            return nameof(Role.Code);
        }
    }
}
