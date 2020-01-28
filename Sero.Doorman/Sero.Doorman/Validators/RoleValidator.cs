using FluentValidation;
using Sero.Doorman.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Doorman.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public readonly IResourceStore ResourceStore;

        public RoleValidator(IResourceStore resourceStore)
        {
            this.ResourceStore = resourceStore;

            RuleFor(x => x.Code)
                .NotEmpty()
                .IsCode();

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .IsDisplayName();

            RuleFor(x => x.Description)
                .IsDescription();

            RuleFor(x => x.Permissions)
                .NotEmpty()
                .Must(BeUniquePermissionPerResource)
                .MustAsync(BeExistingResourceCodes);
        }

        private bool BeUniquePermissionPerResource(List<Permission> permissionList)
        {
            bool hasDuplicateResource = permissionList
                .Select(x => x.ResourceCode)
                .GroupBy(x => x)
                .Any(x => x.Count() > 1);

            return hasDuplicateResource;
        }

        private async Task<bool> BeExistingResourceCodes(List<Permission> permissionList, CancellationToken token)
        {
            foreach(var permission in permissionList)
            {
                bool isExisting = await ResourceStore.IsExistingAsync(permission.ResourceCode);
                if(!isExisting)
                    return false;
            }

            return true;
        }
    }
}
