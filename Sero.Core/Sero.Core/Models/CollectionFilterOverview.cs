//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Sero.Core
//{
//    public class CollectionFilterOverview
//    {
//        public PageCriteria Page;
//        public PageSizeCriteria PageSize;
//        public SortingCriteria SortBy;
//        public OrderingCriteria OrderBy;
//        public FreeTextCriteria FreeText;
//        public List<BaseFilterCriteria> CustomCriteriaList;

//        public CollectionFilterOverview(
//            PageCriteria page,
//            PageSizeCriteria pageSize,
//            SortingCriteria sortBy,
//            OrderingCriteria orderBy,
//            FreeTextCriteria freeText,
//            List<BaseFilterCriteria> customCriteriaList)
//        {
//            this.Page = page;
//            this.PageSize = pageSize;
//            this.SortBy = sortBy;
//            this.OrderBy = orderBy;
//            this.FreeText = freeText;
//            this.CustomCriteriaList = customCriteriaList;
//        }

//        public CollectionFilterOverview CopyAsPage(int newPage)
//        {
//            return new CollectionFilterOverview(new PageCriteria(newPage), PageSize, SortBy, OrderBy, FreeText, CustomCriteriaList);
//        }

//        public List<KeyValuePair<string, string>> GetFilteringArguments()
//        {            
//            List<KeyValuePair<string, string>> flattened = new List<KeyValuePair<string, string>>();

//            var pagePair = Page.GetUrlFriendlyMap();
//            flattened.AddRange(pagePair);

//            var pageSizePair = PageSize.GetUrlFriendlyMap();
//            flattened.AddRange(pageSizePair);

//            var sortByPair = SortBy.GetUrlFriendlyMap();
//            flattened.AddRange(sortByPair);

//            var orderByPair = OrderBy.GetUrlFriendlyMap();
//            flattened.AddRange(orderByPair);

//            var freeTextPair = FreeText.GetUrlFriendlyMap();
//            flattened.AddRange(freeTextPair);

//            foreach (var customCriteria in CustomCriteriaList)
//            {
//                var customPairs = customCriteria.GetUrlFriendlyMap();
//                flattened.AddRange(customPairs);
//            }

//            return flattened;
//        }
//    }
//}
