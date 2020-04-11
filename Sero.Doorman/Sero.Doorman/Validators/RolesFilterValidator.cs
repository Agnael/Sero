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
    public class RolesFilterValidator : AbstractValidator<RoleFilter>
    {
        public RolesFilterValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(50);
        }
    }
}
