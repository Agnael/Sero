using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class DefaultsManagerBuilder<TCollectionFilter>
    {
        private TCollectionFilter _currentFilter;
        private Dictionary<string, object> _defaultsMap;

        public DefaultsManagerBuilder(TCollectionFilter currentFilterInstance)
        {
            if (currentFilterInstance == null) throw new ArgumentNullException(nameof(currentFilterInstance));

            _currentFilter = currentFilterInstance;
            _defaultsMap = new Dictionary<string, object>();
        }

        public U Configure<U>(Expression<Func<TCollectionFilter, U>> propertySelector, U defaultValue)
        {
            string propertyName = ReflectionUtils.GetPropertyName(propertySelector);

            if (_defaultsMap.ContainsKey(propertyName))
                throw new Exception("Ya se había definido un valor por defecto para esta property.");

            _defaultsMap.Add(propertyName, defaultValue);
            _currentFilter.SetPropertyValue(propertySelector, defaultValue);

            return defaultValue;
        }

        public DefaultsManager<TCollectionFilter> Build()
        {
            var provider = new DefaultsManager<TCollectionFilter>(_defaultsMap);
            return provider;
        }
    }
}
