using System.Collections.Generic;

namespace MyAlloySite.Constant
{
    public class Constants
    {
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
            //Double = 24,
            //Integer = 25,
        }

        public static Dictionary<int, string> PromotionTypeDic = new Dictionary<int, string>
            {
                { (int)PromotionType.BuyItemsGetGifts, "Free Gift" },
                { (int)PromotionType.SaleOff, "Sale Off" }
            };
    }
}