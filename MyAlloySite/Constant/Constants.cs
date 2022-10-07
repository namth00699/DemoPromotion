using System.Collections.Generic;

namespace MyAlloySite.Constant
{
    public class Constants
    {
        public const string IndexCategoriesProduct = "IndexCategoriesProduct";

        public enum FilterType
        {
            PromotionType = 1,
            PriceType = 2
        }

        public enum PromotionType
        {
            BuyItemsGetGifts = 1,
            SaleOff = 2
        }

        public enum FacetType
        {
            Term,
            Filter,
            Range
           
        }

        public static Dictionary<int, string> PromotionTypeDic = new Dictionary<int, string>
            {
                { (int)PromotionType.BuyItemsGetGifts, "Free Gift" },
                { (int)PromotionType.SaleOff, "Sale Off" }
            };
    }
}