using FluentValidation;
using Sero.Core;
using Sero.Gatekeeper.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Validators
{
    public class ResourceUpdateFormValidator : AbstractValidator<ResourceUpdateForm>
    {
        public ResourceUpdateFormValidator(
            ICodeRule codeRule,
            IDescriptionRule descriptionRule)
        {
            RuleFor(x => x.Category)
                .ApplyRule(codeRule);

            RuleFor(x => x.Description)
                .ApplyRule(descriptionRule);
        }
    }
}
