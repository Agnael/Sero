using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    /// <summary>
    ///     Marcar con este atributo, dentro de una Action marcada como Getter, el atributo puntual que representa la key por la que se busca.
    /// </summary>
    public class GetterParameterAttribute : Attribute
    {
        public GetterParameterAttribute()
        {
        }
    }
}
