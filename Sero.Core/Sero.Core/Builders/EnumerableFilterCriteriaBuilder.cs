using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class EnumerableFilterCriteriaBuilder<TCollectionFilter, TProperty> : IFilterCriteriaBuilder
    {
        private TCollectionFilter _currentInstance;
        private Expression<Func<TCollectionFilter, IEnumerable<TProperty>>> _filterPropertySelector;
        private string _customPropertyName;
        private BaseFilterCriteria<TProperty> _criteria;

        private IEnumerable<TProperty> _defaultValues;

        public EnumerableFilterCriteriaBuilder(
            TCollectionFilter currentInstance,
            Expression<Func<TCollectionFilter, IEnumerable<TProperty>>> propertySelector)
        {
            _currentInstance = currentInstance;
            _filterPropertySelector = propertySelector;

            _customPropertyName = ReflectionUtils.GetPropertyName(propertySelector);
        }

        public EnumerableFilterCriteriaBuilder<TCollectionFilter, TProperty> UseDefaultValue(IEnumerable<TProperty> defaultValues)
        {
            _defaultValues = new List<TProperty>(defaultValues);
            _currentInstance.SetPropertyValue(_filterPropertySelector, new List<TProperty>(defaultValues));
            return this;
        }

        public EnumerableFilterCriteriaBuilder<TCollectionFilter, TProperty> UseCriteria<TCriteria>()
            where TCriteria : BaseFilterCriteria<TProperty>
        {
            _criteria = Activator.CreateInstance<TCriteria>();
            return this;
        }

        public EnumerableFilterCriteriaBuilder<TCollectionFilter, TProperty> UseCriteria(BaseFilterCriteria<TProperty> criteria)
        {
            _criteria = criteria;
            return this;
        }

        public EnumerableFilterCriteriaBuilder<TCollectionFilter, TProperty> UsePropertyName(string propertyName)
        {
            _customPropertyName = propertyName;
            return this;
        }

        IFilterCriteria IFilterCriteriaBuilder.Build(object value)
        {
            return this.Build((IEnumerable<TProperty>)value);
        }

        public IFilterCriteria Build(IEnumerable<TProperty> values)
        {
            _criteria.SetDefaultValues(_defaultValues);
            _criteria.SetValues(values);

            if (!string.IsNullOrEmpty(_customPropertyName))
                _criteria.SetPropertyName(_customPropertyName);

            return _criteria;
        }
    }
}
