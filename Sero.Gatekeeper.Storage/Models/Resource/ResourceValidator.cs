using FluentValidation;
using Sero.Core;
using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper.Storage
{
    public class ResourceValidator : AbstractValidator<Resource>
    {
        public ResourceValidator(
            ICodeRule codeRule,
            IDescriptionRule descriptionRule)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .ApplyRule(codeRule);

            RuleFor(x => x.Category)
                .NotEmpty()
                .ApplyRule(codeRule);

            RuleFor(x => x.Description)
                .ApplyRule(descriptionRule);
        }
    }
}
