using FluentValidation;
using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Storage
{
    public class ResourceFilterValidator : AbstractValidator<ResourceFilter>
    {
        public ResourceFilterValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
