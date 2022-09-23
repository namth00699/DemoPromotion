using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.Extensions
{
    public class PromotionProductModel
    {
        public decimal ActualPrice { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal SavedAmount { get; set; }

        public decimal GreatestPercent { get; set; }

        public string Currency { get; set; }

        public List<string> RewardType { get; set; }

        public List<string> Status { get; set; }

        public List<decimal> ListPercent { get; set; }

        public List<decimal> ListSavedAmount { get; set; }

        public void SetOriginalValue()
        {
            SetOriginalPrice();
            SetSavedAmount();
            SetGreatestPercent();
        }

        private void SetOriginalPrice()
        {
            var actualPrice = ActualPrice != null ? ActualPrice : 0;
            OriginalPrice = (decimal)(actualPrice + ListSavedAmount.Sum());
        }

        public void SetSavedAmount()
        {
            SavedAmount = OriginalPrice - ActualPrice;
        }
        public void SetGreatestPercent()
        {
            GreatestPercent = ListPercent != null && ListPercent.Any() ? ListPercent.Max() : 0;
        }
    }
}