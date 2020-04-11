using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class PageSizeCriteria : BaseFilterCriteria<int>
    {
        protected override string DefaultPropertyName => "PageSize";

        public override Func<int, string> UrlFriendlyValueTransformer => value => value.ToFilterString();
    }
}
