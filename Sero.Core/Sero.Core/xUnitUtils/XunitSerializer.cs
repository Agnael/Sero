using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit.Abstractions;

namespace Sero.Core
{
    public class XunitSerializer<TObj>
    {
        private List<IXunitSerializationManager<TObj>> _serializationCommands;

        public XunitSerializer()
        {
            _serializationCommands = new List<IXunitSerializationManager<TObj>>();
        }

        public void RegisterField<TProp>(Expression<Func<TObj, IEnumerable<TProp>>> propSelector)
        {
            _serializationCommands.Add(new XunitEnumerableSerializationManager<TObj, TProp>(propSelector));
        }

        public void RegisterField<TProp>(Expression<Func<TObj, TProp>> propSelector)
        {
            _serializationCommands.Add(new XunitSimpleSerializationManager<TObj, TProp>(propSelector));
        }

        public virtual void Deserialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            foreach (var command in _serializationCommands)
                command.Deserialize(info, objInstanceRef);
        }

        public virtual void Serialize(IXunitSerializationInfo info, TObj objInstanceRef)
        {
            foreach (var command in _serializationCommands)
                command.Serialize(info, objInstanceRef);
        }
    }
}
