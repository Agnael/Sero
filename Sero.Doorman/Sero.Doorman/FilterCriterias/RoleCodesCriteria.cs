using Sero.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Doorman
{
    public class RoleCodesCriteria : BaseFilterCriteria<string>
    {
        protected override string DefaultPropertyName => "Roles";

        public override Func<string, string> UrlFriendlyValueTransformer => value => value;
    }
}
