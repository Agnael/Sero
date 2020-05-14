using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Gatekeeper.Storage
{
    public class RoleFilter : BaseCollectionFilter<RoleFilter, RoleSorting>
    {
        protected override RoleFilter CurrentInstance => this;
        protected override RoleSorting DefaultSortBy => RoleSorting.Code;

        public RoleFilter()
        {

        }

        public RoleFilter(int page, int pageSize, RoleSorting sorting, Order ordering, string freeText) 
            : base(page, pageSize, sorting, ordering, freeText)
        {

        }

        protected override void OnConfiguring()
        {

        }
    }
}
