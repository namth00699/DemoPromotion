using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using MyAlloySite.Commerce.Variation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAlloySite.Extensions
{
    public static class VariationContentExtension
    {
        private static readonly IPromotionEngine _promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();
        public static IEnumerable<decimal> IndexPromotion(this ProductVariation product)
        {
            var rewards = _promotionEngine.Evaluate(product.ContentLink);
            return rewards.Select(s => s.SavedAmount);
        }

        public static void SetIndexPromotion(this ProductVariation product)
        {
            //product.;
        }
    }
}