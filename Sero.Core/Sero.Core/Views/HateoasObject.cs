using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public abstract class HateoasObject
    {
        public Dictionary<string, string> _links { get; set; }
        public Dictionary<string, HateoasAction> _actions { get; set; }
        public HateoasLabeledLink _parent { get; set; }
        public object _embedded { get; set; }

        public HateoasObject()
        {
            _links = new Dictionary<string, string>();
            _actions = new Dictionary<string, HateoasAction>();
        }

        public HateoasObject(
            Dictionary<string, string> collectionLinks,
            Dictionary<string, HateoasAction> collectionActions,
            IEnumerable<object> embeddedList,
            HateoasLabeledLink parentLink = null)
        {
            this._links = collectionLinks;
            this._actions = collectionActions;
            this._embedded = embeddedList;
            this._parent = parentLink;
        }
    }
}
