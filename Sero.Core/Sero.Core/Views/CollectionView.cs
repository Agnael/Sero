using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public abstract class CollectionView
    {
        public abstract string ResourceCode { get; }
        public readonly CollectionFilter UsedFilter;
        public readonly int TotalExisting;
        public readonly IEnumerable<object> ViewModels;

        public CollectionView(
            CollectionFilter usedFilter,
            int totalExisting,
            IEnumerable<object> viewModels)
        {
            this.UsedFilter = usedFilter;
            this.TotalExisting = totalExisting;
            this.ViewModels = viewModels;
        }
    }

    public class CollectionView<TElement> : CollectionView
        where TElement : Element
    {
        public override string ResourceCode { 
            get
            {
                string resourceCode = Activator.CreateInstance<TElement>().GetAppResourceCode();
                return resourceCode;
            } 
        }

        public CollectionView(
            CollectionFilter usedFilter,
            int totalExisting,
            IEnumerable<object> viewModels)
            : base(usedFilter, totalExisting, viewModels)
        {

        }
    }
}
