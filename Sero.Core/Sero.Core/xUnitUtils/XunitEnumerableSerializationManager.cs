using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Core
{
    public class XunitEnumerableSerializationManager<TObj, TProp> : IXunitSerializationManager<TObj>
    {
        private Expression<Func<TObj, IEnumerable<TProp>>> _propSelector;

        public XunitEnumerableSerializationManager(
            Expression<Func<TObj, IEnumerable<TProp>>> propSelector)
        {
            _propSelector = propSelector;
        }

        public void Serialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            IEnumerable<TProp> propValue = objInstanceRef.GetPropertyValue(_propSelector);
            string propName = objInstanceRef.GetPropertyName(_propSelector);

            TProp[] valueArray = propValue == null || propValue.Count() == 0 ? null : propValue.ToArray();
            info.AddValue(propName, valueArray);
        }

        public void Deserialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            string propName = objInstanceRef.GetPropertyName(_propSelector);
            TProp[] deserializedArray = info.GetValue<TProp[]>(propName);

            if (deserializedArray == null || deserializedArray.Length == 0)
            {
                objInstanceRef.SetPropertyValue(_propSelector, new List<TProp>());
            }
            else
            {
                objInstanceRef.SetPropertyValue(_propSelector, deserializedArray.ToList());
            }
        }
    }
}
