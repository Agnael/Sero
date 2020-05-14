using Force.DeepCloner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace Sero.Core
{
    public abstract class BaseCollectionFilter<TImpl, TSortingEnum> 
        : ICollectionFilter, IFiltrableByOwner, ICloneable

        where TImpl : BaseCollectionFilter<TImpl, TSortingEnum>
        where TSortingEnum : struct, IConvertible, IComparable, IFormattable
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TSortingEnum SortBy { get; set; }
        public Order OrderBy { get; set; }
        public string FreeText { get; set; }
        public string OwnerId { get; set; }

        private BaseFilterCriteria<int> _pageCriteria;
        private BaseFilterCriteria<int> _pageSizeCriteria;
        private Dictionary<string, IFilterCriteriaBuilder> _additionalCriteriaMap;
        private XunitSerializer<TImpl> _xSerializer;

        protected abstract TImpl CurrentInstance { get; }
        protected abstract TSortingEnum DefaultSortBy { get; }
        protected abstract void OnConfiguring();

        public BaseCollectionFilter()
        {
            Init();
        }

        public BaseCollectionFilter(int page, int pageSize, TSortingEnum sorting, Order ordering, string freeText)
        {
            Init();

            this.Page = page;
            this.PageSize = pageSize;
            this.SortBy = sorting;
            this.OrderBy = ordering;
            this.FreeText = freeText;
        }

        private void Init()
        {
            _xSerializer = new XunitSerializer<TImpl>();
            _additionalCriteriaMap = new Dictionary<string, IFilterCriteriaBuilder>();

            _pageCriteria = new DefaultCriteria<int>(CurrentInstance.GetPropertyName(x => x.Page), false);
            _xSerializer.RegisterField(x => x.Page);
            _pageSizeCriteria = new DefaultCriteria<int>(CurrentInstance.GetPropertyName(x => x.PageSize), false);
            _xSerializer.RegisterField(x => x.PageSize);

            SetDefaultPage(1);
            SetDefaultPageSize(10);

            For(x => x.SortBy)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(DefaultSortBy);

            For(x => x.OrderBy)
                .UseTransformer(x => x.ToUrlFriendlyValue())
                .UseDefaultValue(Order.Asc);

            For(x => x.FreeText)
                .UseDefaultValue(null);

            For(x => x.OwnerId)
                .UseDefaultValue(null);

            OnConfiguring();
        }

        public void SetRequiredOwnerId(string ownerCredentialId)
        {
            OwnerId = ownerCredentialId;
        }

        protected void SetDefaultPage(int defaultPage)
        {
            Page = defaultPage;
            _pageCriteria.SetDefaultValues(defaultPage);
        }

        protected void SetDefaultPageSize(int defaultPageSize)
        {
            PageSize = defaultPageSize;
            _pageSizeCriteria.SetDefaultValues(defaultPageSize);
        }

        protected EnumerableFilterCriteriaBuilder<TImpl, TProp> For<TProp>(Expression<Func<TImpl, IEnumerable<TProp>>> propSelector)
        {
            string propName = ReflectionUtils.GetPropertyName(propSelector);
            var criteriaBuilder = new EnumerableFilterCriteriaBuilder<TImpl, TProp>(CurrentInstance, propSelector);
            _additionalCriteriaMap.Add(propName, criteriaBuilder);
            _xSerializer.RegisterField(propSelector);

            return criteriaBuilder;
        }

        protected SimpleFilterCriteriaBuilder<TImpl, TProp> For<TProp>(Expression<Func<TImpl, TProp>> propSelector)
        {
            string propName = ReflectionUtils.GetPropertyName(propSelector);
            var criteriaBuilder = new SimpleFilterCriteriaBuilder<TImpl, TProp>(CurrentInstance, propSelector);
            _additionalCriteriaMap.Add(propName, criteriaBuilder);
            _xSerializer.RegisterField(propSelector);

            return criteriaBuilder;
        }

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            _xSerializer.Deserialize(info, CurrentInstance);
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            _xSerializer.Serialize(info, CurrentInstance);
        }

        public FilteringOverview GetOverview()
        {
            _pageCriteria.SetValues(Page);
            _pageSizeCriteria.SetValues(PageSize);

            var additionalCriterias = new List<IFilterCriteria>();
            foreach (var kvp in _additionalCriteriaMap)
            {
                IFilterCriteriaBuilder criteriaBuilder = kvp.Value;
                object filtersPropertyValue = ReflectionUtils.GetPropertyValue(CurrentInstance, kvp.Key);

                IFilterCriteria criteria = criteriaBuilder.Build(filtersPropertyValue);
                additionalCriterias.Add(criteria);
            }

            return new FilteringOverview(_pageCriteria, _pageSizeCriteria, additionalCriterias);
        }

        object ICloneable.Clone()
        {
            object cloned = CurrentInstance.DeepClone();
            return cloned;
        }

        /// <summary>
        ///     Default cloning helper, using the DeepClone() method provided by the 'DeepCloner' NuGet package.
        ///     Override for a custom implementation.
        /// </summary>
        public virtual TImpl Clone()
        {
            TImpl cloned = (TImpl)(this as ICloneable).Clone();
            return cloned;
        }
    }
}
