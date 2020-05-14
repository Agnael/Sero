using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public abstract class CollectionView : IResultView
    {
        public abstract string ResourceCode { get; }
        public readonly FilteringOverview UsedFilter;
        public readonly int TotalExisting;
        public readonly IEnumerable<IApiResource> ViewModels;

        public CollectionView(
            FilteringOverview usedFilter,
            int totalExisting,
            IEnumerable<IApiResource> viewModels)
        {
            this.UsedFilter = usedFilter;
            this.TotalExisting = totalExisting;
            this.ViewModels = viewModels;
        }
    }

    public class CollectionView<TElement> : CollectionView
        where TElement : IApiResource
    {
        public override string ResourceCode { 
            get
            {
                string resourceCode = Activator.CreateInstance<TElement>().ApiResourceCode;
                return resourceCode;
            } 
        }

        public CollectionView(
            FilteringOverview usedFilter,
            int totalExisting,
            IEnumerable<IApiResource> viewModels)
            : base(usedFilter, totalExisting, viewModels)
        {

        }
    }
}
