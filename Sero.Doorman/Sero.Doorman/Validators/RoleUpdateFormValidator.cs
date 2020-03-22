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
    public class RoleUpdateFormValidator : AbstractValidator<RoleUpdateForm>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RoleUpdateFormValidator(
            IRoleStore roleStore, 
            IResourceStore resourceStore)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .IsDisplayName();

            RuleFor(x => x.Description)
                .IsDescription();

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
                bool isExisting = await ResourceStore.IsUnique(permission.ResourceCode);

                if (!isExisting)
                    return false;
            }

            return true;
        }
    }
}
