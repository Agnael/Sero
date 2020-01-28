using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    /// <summary>
    ///     Sirve para marcar las acciones que sirvan para afectar un elemento puntual de una colección (por ejemplo, editarlo o eliminarlo)
    /// </summary>
    public class OnElementAttribute : Attribute
    {
        public readonly string ResourceCode;

        public OnElementAttribute(string resourceCode)
        {
            this.ResourceCode = resourceCode;
        }
    }
}
