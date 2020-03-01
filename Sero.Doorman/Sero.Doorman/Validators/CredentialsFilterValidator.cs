using FluentValidation;
using Sero.Doorman.Controller;
using Sero.Doorman.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Doorman.Validators
{
    public class CredentialsFilterValidator : AbstractValidator<CredentialsFilter>
    {
        public CredentialsFilterValidator()
        {
            RuleFor(x => x.SortBy)
                .IsCSharpPropertyName()
                .IsPropertyNameOf(typeof(Credential));

            RuleFor(x => x.OrderBy)
                .IsOrderingDescriptor();

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
