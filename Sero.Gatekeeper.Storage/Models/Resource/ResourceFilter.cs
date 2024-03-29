﻿using Sero.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Gatekeeper.Storage
{
    public class ResourceFilter : BaseCollectionFilter<ResourceFilter, ResourceSorting>
    {
        protected override ResourceFilter CurrentInstance => this;
        protected override ResourceSorting DefaultSortBy => ResourceSorting.Code;

        public ResourceFilter()
        {
        }

        public ResourceFilter(int page, int pageSize, ResourceSorting sorting, Order ordering, string freeText) 
            : base(page, pageSize, sorting, ordering, freeText)
        {
        }

        protected override void OnConfiguring()
        {

        }
    }
}
