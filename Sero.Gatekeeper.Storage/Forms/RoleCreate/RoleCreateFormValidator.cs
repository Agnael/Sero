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
    public class RoleCreateFormValidator : AbstractValidator<RoleCreateForm>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RoleCreateFormValidator(
            IRoleStore roleStore, 
            IResourceStore resourceStore,
            ICodeRule codeRule,
            IDescriptionRule descriptionRule,
            IDisplayNameRule displayNameRule)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;

            RuleFor(x => x.Code)
                .NotEmpty()
                .ApplyRule(codeRule)
                .MustAsync(NotBeExistingCodeAsync)
                .WithMessage("Already existing");

            RuleFor(x => x.Name)
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

        private async Task<bool> NotBeExistingCodeAsync(string roleCode, CancellationToken cancelToken)
        {
            bool isExisting = await RoleStore.IsExisting(roleCode);
            return !isExisting;
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
