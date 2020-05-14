using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    public class FilteringOverview
    {
        public readonly BaseFilterCriteria<int> Page;
        public readonly BaseFilterCriteria<int> PageSize;

        public readonly IEnumerable<IFilterCriteria> AdditionalCriterias;

        public FilteringOverview(
            BaseFilterCriteria<int> page,
            BaseFilterCriteria<int> pageSize,
            IEnumerable<IFilterCriteria> additionalCriterias)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.AdditionalCriterias = additionalCriterias;
        }

        public FilteringOverview CopyAsPage(int newPage)
        {
            BaseFilterCriteria<int> newPageCriteria = new DefaultCriteria<int>(this.Page.Name, Page.HasMultipleValues);
            newPageCriteria.SetDefaultValues(Page.DefaultValues);
            newPageCriteria.SetValues(newPage);

            return new FilteringOverview(newPageCriteria, PageSize, AdditionalCriterias);
        }
    }
}
