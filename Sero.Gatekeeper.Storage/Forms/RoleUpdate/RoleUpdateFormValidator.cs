using FluentValidation;
using Sero.Core;
using Sero.Gatekeeper.Controller;
using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Validators
{
    public class RoleUpdateFormValidator : AbstractValidator<RoleUpdateForm>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RoleUpdateFormValidator(
            IRoleStore roleStore, 
            IResourceStore resourceStore,
            IDisplayNameRule displayNameRule,
            IDescriptionRule descriptionRule)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .ApplyRule(displayNameRule);

            RuleFor(x => x.Description)
                .ApplyRule(descriptionRule);

            RuleFor(x => x.Permissions)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MustAsync(BeExistingResourceCodes)
                .WithMessage("Unexisting resource code detected.");
        }

        private async Task<bool> BeExistingResourceCodes(IEnumerable<Permission> permissionList, CancellationToken token)
        {
            foreach (var permission in permissionList)
            {
                bool isExisting = await ResourceStore.IsExisting(permission.ResourceCode);

                if (!isExisting)
                    return false;
            }

            return true;
        }
    }
}
