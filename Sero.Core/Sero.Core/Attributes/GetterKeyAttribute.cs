using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    /// <summary>
    ///     Usar este attribute para marcar properties de cualquier clase que pueda usarse como reemplazo en el template de la URL
    ///     del endpoint marcado como [Getter] del ResourceCode indicado.
    /// </summary>
    public class GetterKeyAttribute : Attribute
    {
        /// <summary>
        ///     ResourceCode para el que este atributo representa la key principal
        /// </summary>
        public readonly string ResourceCode;

        public GetterKeyAttribute(string elementGetterResourceCode)
        {
            this.ResourceCode = elementGetterResourceCode;
        }
    }
}
