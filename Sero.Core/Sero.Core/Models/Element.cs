using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    /// <summary>
    ///     Every hateoas must inherit from this class. It binds a resourceCode with a specific class.
    /// </summary>
    public abstract class Element
    {
        private readonly string _resourceCode;

        public Element(string resourceCode)
        {
            _resourceCode = resourceCode;
        }

        public string GetAppResourceCode()
        {
            return _resourceCode;
        }
    }
}
