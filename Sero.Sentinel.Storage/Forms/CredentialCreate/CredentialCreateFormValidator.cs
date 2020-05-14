using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Sero.Core;
using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sero.Sentinel.Storage
{
    public class CredentialCreateFormValidator : AbstractValidator<CredentialCreateForm>
    {
        public readonly ICredentialStore CredentialStore;

        public CredentialCreateFormValidator(
            ICredentialStore credentialStore,
            IDisplayNameRule displayNameRule)
        {
            this.CredentialStore = credentialStore;

            RuleFor(x => x.Birthdate)
                .NotEmpty()
                .InclusiveBetween(DateTime.UtcNow.AddYears(-120), DateTime.UtcNow);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(NotBeExistingEmail)
                    .WithMessage("Already registered");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty()
                .MinimumLength(8)
                .Must((form, passwordConfirmation) => passwordConfirmation == form.Password)
                    .WithMessage("Passwords must be equal");

            RuleFor(x => x.Username)
                .NotEmpty()
                .ApplyRule(displayNameRule)
                .MustAsync(NotBeExistingCredentialId);
        }

        private async Task<bool> NotBeExistingCredentialId(string CredentialId, CancellationToken cancelToken)
        {
            bool isExisting = await CredentialStore.IsExistingByCredentialId(CredentialId);
            return !isExisting;
        }

        private async Task<bool> NotBeExistingEmail(string email, CancellationToken cancelToken)
        {
            bool isExisting = await CredentialStore.IsExistingByEmail(email);
            return !isExisting;
        }
    }
}
