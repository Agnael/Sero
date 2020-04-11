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

        IEnumerable<object> DefaultValues { get; }
        IEnumerable<object> Values { get; }

        //void SetDefaultValues(IEnumerable<object> defaultValues);
        //void SetValues(IEnumerable<object> values);

        Func<object, string> UrlFriendlyValueTransformer { get; }
    }

    public abstract class BaseFilterCriteria<T> : IFilterCriteria
    {
        protected abstract string DefaultPropertyName { get; }
        public abstract Func<T, string> UrlFriendlyValueTransformer { get; }

        public string Name => _customPropertyName ?? DefaultPropertyName;

        IEnumerable<object> IFilterCriteria.DefaultValues => this.DefaultValues.Cast<object>();
        IEnumerable<object> IFilterCriteria.Values => this.Values.Cast<object>();

        public IEnumerable<T> DefaultValues { get; private set; }
        public IEnumerable<T> Values { get; private set; }

        Func<object, string> IFilterCriteria.UrlFriendlyValueTransformer => obj => this.UrlFriendlyValueTransformer((T)obj);
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

        public void SetPropertyName(string propertyName)
        {
            _customPropertyName = propertyName;
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
