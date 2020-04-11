using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class FreeDateCriteria : BaseFilterCriteria<DateTime?>
    {
        protected override string DefaultPropertyName => "FreeText";
        
        public override Func<DateTime?, string> UrlFriendlyValueTransformer => value => value.ToFilterString();
    }
}
