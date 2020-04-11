using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class PageCriteria : BaseFilterCriteria<int>
    {
        protected override string DefaultPropertyName => "Page";

        public override Func<int, string> UrlFriendlyValueTransformer => value => value.ToFilterString();
    }
}
