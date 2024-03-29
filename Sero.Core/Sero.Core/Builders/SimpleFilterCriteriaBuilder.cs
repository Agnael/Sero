﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> : IFilterCriteriaBuilder
    {
        private TCollectionFilter _currentInstance;
        private Expression<Func<TCollectionFilter, TProperty>> _filterPropertySelector;
        private TProperty _defaultValue;
        private BaseFilterCriteria<TProperty> _criteria;
        private string _customPropertyName;
        private Func<TProperty, string> _customTransformer;

        public SimpleFilterCriteriaBuilder(
            TCollectionFilter currentInstance,
            Expression<Func<TCollectionFilter, TProperty>> propertySelector)
        {
            _currentInstance = currentInstance;
            _filterPropertySelector = propertySelector;

            _customPropertyName = ReflectionUtils.GetPropertyName(propertySelector);
            _criteria = new DefaultCriteria<TProperty>(_customPropertyName, false);
        }

        public SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> UseTransformer(Func<TProperty, string> valueToUrlFriendlyStringTransformer)
        {
            _customTransformer = valueToUrlFriendlyStringTransformer;
            return this;
        }

        public SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> UseDefaultValue(TProperty defaultValue)
        {
            _defaultValue = defaultValue;
            _currentInstance.SetPropertyValue(_filterPropertySelector, defaultValue);
            return this;
        }

        public SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> UseCriteria<TCriteria>()
            where TCriteria : BaseFilterCriteria<TProperty>
        {
            _criteria = Activator.CreateInstance<TCriteria>();
            return this;
        }

        public SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> UseCriteria(BaseFilterCriteria<TProperty> criteria)
        {
            _criteria = criteria;
            return this;
        }

        public SimpleFilterCriteriaBuilder<TCollectionFilter, TProperty> UseName(string propertyName)
        {
            _customPropertyName = propertyName;
            return this;
        }

        IFilterCriteria IFilterCriteriaBuilder.Build(object value)
        {
            return this.Build((TProperty)value);
        }

        public IFilterCriteria Build(TProperty definitiveValue)
        {
            _criteria.SetDefaultValues(_defaultValue);
            _criteria.SetValues(definitiveValue);

            if (_customTransformer != null)
                _criteria.SetUrlFriendlyTransformer(_customTransformer);

            if (!string.IsNullOrEmpty(_customPropertyName))
                _criteria.SetPropertyName(_customPropertyName);

            return _criteria;
        }
    }
}
