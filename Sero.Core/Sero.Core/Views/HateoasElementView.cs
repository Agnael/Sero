using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class HateoasElementView : HateoasObject
    {
        public HateoasElementView(
            Dictionary<string, string> elementLinks,
            Dictionary<string, HateoasAction> elementActions,
            object element)
        {
            this._links = elementLinks;
            this._actions = elementActions;
            this._embedded = element;
        }
    }
}
