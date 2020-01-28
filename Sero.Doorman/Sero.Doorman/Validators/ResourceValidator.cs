using FluentValidation;
using Sero.Doorman.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Validators
{
    public class ResourceValidator : AbstractValidator<Resource>
    {
        public ResourceValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .IsCode();

            RuleFor(x => x.Category)
                .NotEmpty()
                .IsCode();

            RuleFor(x => x.Description)
                .IsDescription();
        }
    }
}
