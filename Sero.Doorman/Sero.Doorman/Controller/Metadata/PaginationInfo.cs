using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman.Controller
{
    public class PaginationInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
    }
}
