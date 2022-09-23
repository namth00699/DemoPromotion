using System.Collections.Generic;

namespace MyAlloySite.Constant
{
    public static class GlobalValues
    {
        public const string Default = "Default";
        public const string NameDes = "name_desc";
        public const string NameAsc = "name_asc";
        public const string PercentDesc = "percent_desc";
        public const string PercentAsc = "percent_asc";
        public const string PriceDesc = "price_desc";
        public const string PriceAsc = "price_asc";

        public static Dictionary<string, string> SortingDictionary = new Dictionary<string, string>
        {
            {"Default", GlobalValues.Default },
            {"Name Descending", NameDes },
            {"Name Ascending", NameAsc },
            {"Percent Descending", PercentDesc },
            {"Percent Ascending", PercentAsc },
            {"Price Descending", PriceDesc },
            {"Price Ascending", PriceAsc },
        };
    }
}