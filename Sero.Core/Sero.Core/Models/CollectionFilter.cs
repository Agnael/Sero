using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace Sero.Core
{
    public abstract class CollectionFilter<TImpl, TSortingEnum> : ICollectionFilter
        where TImpl : CollectionFilter<TImpl, TSortingEnum>
        where TSortingEnum : struct, IConvertible, IComparable, IFormattable
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TSortingEnum SortBy { get; set; }
        public Order OrderBy { get; set; }
        public string FreeText { get; set; }

        private PageCriteria _pageCriteria;
        private PageSizeCriteria _pageSizeCriteria;
        private Dictionary<string, IFilterCriteriaBuilder> _additionalCriteriaMap;

        public abstract void XunitDeserialize(IXunitSerializationInfo info);
        public abstract void XunitSerialize(IXunitSerializationInfo info);
        protected abstract TImpl CurrentInstance { get; }
        protected abstract TSortingEnum DefaultSortBy { get; }
        protected abstract void OnConfiguring();

        public CollectionFilter()
        {
            Init();
        }

        public CollectionFilter(int page, int pageSize, TSortingEnum sorting, Order ordering, string freeText)
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
            _additionalCriteriaMap = new Dictionary<string, IFilterCriteriaBuilder>();

            _pageCriteria = new PageCriteria();
            _pageSizeCriteria = new PageSizeCriteria();

            SetDefaultPage(1);
            SetDefaultPageSize(10);

            For(x => x.SortBy)
                .UseCriteria<SortingCriteria<TSortingEnum>>()
                .UseDefaultValue(DefaultSortBy);

            For(x => x.OrderBy)
                .UseCriteria<OrderingCriteria>()
                .UseDefaultValue(Order.Asc);

            For(x => x.FreeText)
                .UseCriteria<FreeTextCriteria>()
                .UseDefaultValue(null);

            OnConfiguring();
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

        protected EnumerableFilterCriteriaBuilder<TImpl, SelectedPropType> For<SelectedPropType>(Expression<Func<TImpl, IEnumerable<SelectedPropType>>> propertySelector)
        {
            string propName = ReflectionUtils.GetPropertyName(propertySelector);
            var criteriaBuilder = new EnumerableFilterCriteriaBuilder<TImpl, SelectedPropType>(CurrentInstance, propertySelector);
            _additionalCriteriaMap.Add(propName, criteriaBuilder);

            return criteriaBuilder;
        }

        protected SimpleFilterCriteriaBuilder<TImpl, SelectedPropType> For<SelectedPropType>(Expression<Func<TImpl, SelectedPropType>> propertySelector)
        {
            string propName = ReflectionUtils.GetPropertyName(propertySelector);
            var criteriaBuilder = new SimpleFilterCriteriaBuilder<TImpl, SelectedPropType>(CurrentInstance, propertySelector);
            _additionalCriteriaMap.Add(propName, criteriaBuilder);

            return criteriaBuilder;
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

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            this.FreeText = info.GetValue<string>(nameof(FreeText));

            this.OrderBy = info.GetValue<Order>(nameof(OrderBy));
            this.SortBy = info.GetValue<TSortingEnum>(nameof(SortBy));

            this.Page = info.GetValue<int>(nameof(Page));
            this.PageSize = info.GetValue<int>(nameof(PageSize));

            this.XunitDeserialize(info);
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(FreeText), this.FreeText);

            info.AddValue(nameof(OrderBy), this.OrderBy);
            info.AddValue(nameof(SortBy), this.SortBy);

            info.AddValue(nameof(Page), this.Page);
            info.AddValue(nameof(PageSize), this.PageSize);

            this.XunitSerialize(info);
        }
    }
}
