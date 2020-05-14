using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core
{
    public delegate string UrlFriendlyTransformer(object value);

    public interface IFilterCriteria
    {
        string Name { get; }
        bool IsModified { get; }
        bool HasMultipleValues { get; }

        IEnumerable<object> DefaultValues { get; }
        IEnumerable<object> Values { get; }

        string UrlFriendlyValueTransformer(object value);
    }

    public abstract class BaseFilterCriteria<T> : IFilterCriteria
    {
        protected abstract string DefaultPropertyName { get; }
        public abstract bool HasMultipleValues { get; }

        public abstract string UrlFriendlyValueTransformerDefault(T value);
        private Func<T,string> _urlFriendlyValueTransformerOverride;

        public string Name => _customPropertyName ?? DefaultPropertyName;

        IEnumerable<object> IFilterCriteria.DefaultValues => this.DefaultValues.Cast<object>();
        IEnumerable<object> IFilterCriteria.Values => this.Values.Cast<object>();

        public IEnumerable<T> DefaultValues { get; private set; }
        public IEnumerable<T> Values { get; private set; }
               
        private string _customPropertyName;

        protected BaseFilterCriteria()
        {
            DefaultValues = new List<T>();
            Values = new List<T>();
        }
        
        public bool IsModified
        {
            get
            {
                bool isValueUnchanged = DefaultValues.SequenceEqual(Values);
                return !isValueUnchanged;
            }
        }

        string IFilterCriteria.UrlFriendlyValueTransformer(object value)
        {
            if (_urlFriendlyValueTransformerOverride != null)
                return _urlFriendlyValueTransformerOverride((T)value);

            return this.UrlFriendlyValueTransformerDefault((T)value);
        }

        public void SetPropertyName(string propertyName)
        {
            _customPropertyName = propertyName;
        }

        public void SetUrlFriendlyTransformer(Func<T, string> valueTransformer)
        {
            _urlFriendlyValueTransformerOverride = valueTransformer;
        }

        public void SetDefaultValues(IEnumerable<T> defaultValues)
        {
            DefaultValues = defaultValues;
        }

        public void SetValues(IEnumerable<T> newValues)
        {
            Values = newValues;
        }

        public void SetDefaultValues(T defaultValue)
        {
            DefaultValues = new List<T> { defaultValue };
        }

        public void SetValues(T value)
        {
            Values = new List<T> { value };
        }
    }
}
