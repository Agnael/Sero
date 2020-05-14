using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Core
{
    public class XunitSimpleSerializationManager<TObj, TProp> : IXunitSerializationManager<TObj>
    {
        private Expression<Func<TObj, TProp>> _propSelector;

        public XunitSimpleSerializationManager(
            Expression<Func<TObj, TProp>> propSelector)
        {
            _propSelector = propSelector;
        }

        public void Serialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            string propertyName = objInstanceRef.GetPropertyName(_propSelector);
            TProp propertyValue = objInstanceRef.GetPropertyValue(_propSelector);
            info.AddValue(propertyName, propertyValue);
        }

        public void Deserialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            string propertyName = objInstanceRef.GetPropertyName(_propSelector);
            TProp deserializedValue = info.GetValue<TProp>(propertyName);
            objInstanceRef.SetPropertyValue(_propSelector, deserializedValue);
        }
    }
}
