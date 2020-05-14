using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IBaseRule<TField>
    {
        IRuleBuilder<T, TField> ApplyRule<T>(IRuleBuilder<T, TField> rule);
    }
}
