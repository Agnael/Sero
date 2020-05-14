using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Gatekeeper
{
    //public abstract class BaseOptionsValidator<TOptions> : IValidateOptions<TOptions>
    //{
    //    public ValidateOptionsResult Validate(string name, TOptions options)
    //    {

    //    }

    //    public void RuleFor<TProp>(Expression<Func<TOptions, TProp>> propertySelector)
    //    {

    //    }
    //}

    public class GtkOptionsValidator : IValidateOptions<GtkOptions>
    {
        public GtkOptionsValidator()
        {

        }

        public ValidateOptionsResult Validate(string name, GtkOptions options)
        {
            if (string.IsNullOrEmpty(options.MainSalt))
                return ValidateOptionsResult.Fail("Un MainSalt debe ser configurado.");

            if (options.JwtGeneration == null)
                return ValidateOptionsResult.Fail("No se encontró la configuración de JWT necesaria.");

            if (string.IsNullOrEmpty(options.JwtGeneration.Issuer))
                return ValidateOptionsResult.Fail("Un issuer de JWT debe ser configurado.");

            return ValidateOptionsResult.Success;
        }
    }
}
