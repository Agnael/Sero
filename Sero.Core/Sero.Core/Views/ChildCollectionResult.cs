using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public class ChildCollectionResult
    {
        public readonly object ParentElement;
        public readonly IEnumerable<object> ChildCollection;

        public ChildCollectionResult(
            object parentElement,
            IEnumerable<object> childCollection)
        {
            this.ParentElement = parentElement;
            this.ChildCollection = childCollection;
        }
    }
}
