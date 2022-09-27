using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAlloySite.Api
{
    public class PaginationModel
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 6;

        public bool HasMore { get; set; }
    }
}