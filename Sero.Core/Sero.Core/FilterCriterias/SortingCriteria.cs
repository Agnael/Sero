using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class SortingCriteria<TSortingEnum> : BaseFilterCriteria<TSortingEnum>
        where TSortingEnum : IComparable, IFormattable, IConvertible
    {
        protected override string DefaultPropertyName => "SortBy";

        public override Func<TSortingEnum, string> UrlFriendlyValueTransformer => value => value.ToFilterString();
    }
}
