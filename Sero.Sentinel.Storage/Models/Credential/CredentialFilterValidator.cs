using FluentValidation;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public class CredentialFilterValidator : AbstractValidator<CredentialFilter>
    {
        public CredentialFilterValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
