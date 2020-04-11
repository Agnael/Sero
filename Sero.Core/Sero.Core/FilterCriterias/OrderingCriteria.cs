using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class OrderingCriteria : BaseFilterCriteria<Order>
    {
        protected override string DefaultPropertyName => throw new NotImplementedException();

        public override Func<Order, string> UrlFriendlyValueTransformer => value => value.ToFilterString();
    }
}
