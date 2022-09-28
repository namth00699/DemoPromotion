using EPiServer.ServiceLocation;
using MyAlloySite.Extensions;
using System.Linq;

namespace MyAlloySite.Service
{
    interface IPromotionHelpService
    {
        PromotionProductModel SetOriginalValue(PromotionProductModel model);
    }

    [ServiceConfiguration(typeof(IPromotionHelpService))]
    public class PromotionHelpService : IPromotionHelpService
    {
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
