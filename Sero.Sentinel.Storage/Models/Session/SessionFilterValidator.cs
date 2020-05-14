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
    public class SessionFilterValidator : AbstractValidator<SessionFilter>
    {
        public SessionFilterValidator(IDisplayNameRule displayNameRule)
        {
            // Para el filtrado no tiene sentido aplicar la regla completa de display name, pero si que le 
            // aplica el chequeo de pattern, porque con caracteres ilegales como requeridos solo se van a 
            // generar queries inutiles que consumen recursos y nunca traen resultados
            RuleFor(x => x.CredentialId)
                .Matches(displayNameRule.RegexPattern);

            RuleFor(x => x.ExpirationDateMax)
                .GreaterThan(x => x.ExpirationDateMin);

            RuleFor(x => x.LastActiveDateMax)
                .GreaterThan(x => x.LastActiveDateMin);

            RuleFor(x => x.LoginDateMax)
                .GreaterThan(x => x.LoginDateMin);
        }
    }
}
