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
    public class RoleCreateFormValidator : AbstractValidator<RoleCreateForm>
    {
        public readonly IRoleStore RoleStore;
        public readonly IResourceStore ResourceStore;

        public RoleCreateFormValidator(
            IRoleStore roleStore, 
            IResourceStore resourceStore)
        {
            this.RoleStore = roleStore;
            this.ResourceStore = resourceStore;

            RuleFor(x => x.Code)
                .NotEmpty()
                .IsCode()
                .MustAsync(NotBeExistingCodeAsync)
                .WithMessage("Already existing");

            RuleFor(x => x.Name)
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
