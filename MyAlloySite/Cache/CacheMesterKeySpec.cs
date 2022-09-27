using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAlloySite.Cache
{
    public class CacheMesterKeySpec
    {
        public const string MasterKey = "EluxCache.MasterKey";
        public const string OutputCacheMasterKey = "ContentAwareOutputCache.MasterKey";

        public static readonly TimeSpan DefaultTimeSpan = TimeSpan.FromHours(24);
        public static readonly TimeSpan NavigationTimeSpan = TimeSpan.FromDays(365);

        public class Categories
        {
            public const string Promotion = "EluxCache.Categories.Promotion";
            public const string Campaign = "EluxCache.Categories.Campaign";
        }
    }
}