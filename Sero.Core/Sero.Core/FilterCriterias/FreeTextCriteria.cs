using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class FreeTextCriteria : BaseFilterCriteria<string>
    {
        protected override string DefaultPropertyName => "FreeText";
        
        public override Func<string, string> UrlFriendlyValueTransformer => value => value;

    }
}
