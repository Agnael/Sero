using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    /// <summary>
    ///     Sirve para marcar las acciones que sirvan para afectar una colección como conjunto (por ejemplo, crear un elemento nuevo)
    /// </summary>
    public class OnCollectionAttribute : Attribute
    {
        public readonly string ResourceCode;

        public OnCollectionAttribute(string resourceCode)
        {
            this.ResourceCode = resourceCode;
        }
    }
}
