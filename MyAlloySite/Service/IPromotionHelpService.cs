using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.Promotions;
using EPiServer.ServiceLocation;
using MyAlloySite.Extensions;
using MyAlloySite.Promotions;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.Service
{
    interface IPromotionHelpService
    {
        List<int> GetListPromotionType(List<RewardDescription> promotions);

        PromotionProductModel SetOriginalValue(PromotionProductModel model);
    }

    [ServiceConfiguration(typeof(IPromotionHelpService))]
    public class PromotionHelpService : IPromotionHelpService
    {
        public List<int> GetListPromotionType(List<RewardDescription> promotions)
        {
            var results = new HashSet<int>();
            foreach (var item in promotions)
            {
                if (item.Promotion is BuyItemsGetGifts)
                {
                    results.Add((int)Constant.Constants.PromotionType.BuyItemsGetGifts);
                }
                if (item.Promotion is BuyFromCategoryGetItemDiscount)
                {
                    results.Add((int)Constant.Constants.PromotionType.SaleOff);
                }
            }
            return results.ToList();
        }

        public PromotionProductModel SetOriginalValue(PromotionProductModel model)
        {
            SetOriginalPrice(model);
            SetSavedAmount(model);
            SetGreatestPercent(model);
            return model;
        }

        private void SetOriginalPrice(PromotionProductModel model)
        {
            var actualPrice = model.ActualPrice != null ? model.ActualPrice : 0;
            model.OriginalPrice = (decimal)(actualPrice + model.ListSavedAmount.Sum());
        }

        private void SetSavedAmount(PromotionProductModel model)
        {
            model.SavedAmount = model.OriginalPrice - model.ActualPrice;
        }
        private void SetGreatestPercent(PromotionProductModel model)
        {
            model.GreatestPercent = model.ListPercent != null && model.ListPercent.Any() ? model.ListPercent.Max() : 0;
        }
    }
}
