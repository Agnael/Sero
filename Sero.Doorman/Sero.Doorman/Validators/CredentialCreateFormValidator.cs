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
    public class CredentialCreateFormValidator : AbstractValidator<CredentialCreateForm>
    {
        public readonly ICredentialStore CredentialStore;

        public CredentialCreateFormValidator(ICredentialStore credentialStore)
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

            RuleFor(x => x.CredentialId)
                .NotEmpty()
                .IsDisplayName()
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
