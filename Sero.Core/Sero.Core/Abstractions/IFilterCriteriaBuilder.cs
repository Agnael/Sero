using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public interface IFilterCriteriaBuilder
    {
        IFilterCriteria Build(object value);
    }
}
