using FluentValidation;
using Sero.Core;
using Sero.Gatekeeper.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Gatekeeper.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public readonly IResourceStore ResourceStore;

        public RoleValidator(
            IResourceStore resourceStore,
            ICodeRule codeRule,
            IDisplayNameRule displayNameRule,
            IDescriptionRule descriptionRule)
        {
            this.ResourceStore = resourceStore;

            RuleFor(x => x.Code)
                .NotEmpty()
                .ApplyRule(codeRule);

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .ApplyRule(displayNameRule);

            RuleFor(x => x.Description)
                .ApplyRule(descriptionRule);

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
                bool isExisting = await ResourceStore.IsExisting(permission.ResourceCode);
                
                if(!isExisting)
                    return false;
            }

            return true;
        }
    }
}
