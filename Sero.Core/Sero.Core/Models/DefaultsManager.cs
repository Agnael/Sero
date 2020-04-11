using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class DefaultsManager<TCollectionFilter>
    {
        private Dictionary<string, object> _defaultsMap;

        public DefaultsManager(Dictionary<string, object> defaults)
        {
            _defaultsMap = defaults;
        }

        public bool IsDefault<U>(Expression<Func<TCollectionFilter, U>> propertySelector, U valueCandidate)
        {
            object defaultValue = GetDefault(propertySelector);

            if (defaultValue == null)
            {
                bool isNullCandidate = default(U) == null && valueCandidate == null;
                return isNullCandidate;
            }

            bool isDefaultValue = defaultValue.Equals(valueCandidate);
            return isDefaultValue;
        }

        public U GetDefault<U>(Expression<Func<TCollectionFilter, U>> propertySelector)
        {
            string propertyName = ReflectionUtils.GetPropertyName(propertySelector);

            object value = null;

            if (!_defaultsMap.TryGetValue(propertyName, out value))
                throw new Exception("Se intentó encontrar el valor por defecto de una propiedad del filtro que no tiene definido ningun valor por defecto.");

            return (U)value;
        }

        public TProp GetDefault<TProp>(Expression<Func<TCollectionFilter, object>> propertySelector)
        {
            object value = GetDefault(propertySelector);

            if (!(value is TProp))
                throw new Exception("Se encontró un valor por defecto para la propiedad del filtro pedida, pero no pudo castearse al tipo indicado.");

            return (TProp)value;
        }
    }
}
