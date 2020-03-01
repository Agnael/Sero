using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Core
{
    public class ChildElementResult
    {
        public readonly object ParentElement;
        public readonly object ChildElement;

        public ChildElementResult(
            object parentElement,
            object childElement)
        {
            this.ParentElement = parentElement;
            this.ChildElement = childElement;
        }
    }
}
