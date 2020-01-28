using FluentValidation;
using Sero.Doorman.Controller;
using Sero.Doorman.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Validators
{
    public class ResourceUpdateFormValidator : AbstractValidator<ResourceUpdateForm>
    {
        public ResourceUpdateFormValidator()
        {
            RuleFor(x => x.Category)
                .IsCode();

            RuleFor(x => x.Description)
                .IsDescription();
        }
    }
}
