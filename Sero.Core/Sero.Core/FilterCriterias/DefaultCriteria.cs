using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class DefaultCriteria<TProperty> : BaseFilterCriteria<TProperty>
    {
        private readonly string _defaultName;
        private readonly bool _hasMultipleValues;

        protected override string DefaultPropertyName => _defaultName;
        public override bool HasMultipleValues => _hasMultipleValues;

        public DefaultCriteria(string defaultName, bool hasMultipleValues)
        {
            _defaultName = defaultName;
            _hasMultipleValues = hasMultipleValues;
        }
        
        public override string UrlFriendlyValueTransformerDefault(TProperty value)
        {
            return value.ToString().ToLower();
        }
    }
}
