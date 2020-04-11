using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class FilteringOverview
    {
        public readonly PageCriteria Page;
        public readonly PageSizeCriteria PageSize;

        public readonly IEnumerable<IFilterCriteria> AdditionalCriterias;

        public FilteringOverview(
            PageCriteria page,
            PageSizeCriteria pageSize,
            IEnumerable<IFilterCriteria> additionalCriterias)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.AdditionalCriterias = additionalCriterias;
        }

        public FilteringOverview CopyAsPage(int newPage)
        {
            PageCriteria newPageCriteria = new PageCriteria();
            newPageCriteria.SetDefaultValues(Page.DefaultValues);
            newPageCriteria.SetValues(newPage);

            return new FilteringOverview(newPageCriteria, PageSize, AdditionalCriterias);
        }
    }
}
