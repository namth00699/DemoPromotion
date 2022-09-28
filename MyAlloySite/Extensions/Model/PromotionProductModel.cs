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
    }
}