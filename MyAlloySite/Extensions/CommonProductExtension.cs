using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.ServiceLocation;
using MyAlloySite.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.Extensions
{
    public static class CommonProductExtension
    {
        private static readonly IPromotionEngine _promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();
        private static readonly IContentLoader contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private static readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        public static PromotionProductModel IndexPromotion(this CommonProducts product)
        {
            List<RewardDescription> promotions = GetListRewardDescription(product);

            var promotionProductModel = new PromotionProductModel()
            {
                ActualPrice = (decimal)(promotions?.FirstOrDefault()?.Redemptions?.FirstOrDefault()?.AffectedEntries?.PriceEntries?.FirstOrDefault()?.ActualTotal != null 
                    ? promotions?.FirstOrDefault()?.Redemptions?.FirstOrDefault()?.AffectedEntries?.PriceEntries?.FirstOrDefault()?.ActualTotal : 0),
                ListSavedAmount = promotions.Select(s => s.SavedAmount).ToList(),
                Currency = promotions?.FirstOrDefault()?.Redemptions?.FirstOrDefault()?.AffectedEntries?.PriceEntries?.FirstOrDefault()?.Currency,
                ListPercent = promotions.Select(s => s.Percentage).ToList(),
                RewardType = promotions.Select(s => s.RewardType.ToString()).ToList(),
                Status = promotions.Select(s => s.Status.ToString()).ToList(),
            };
            promotionProductModel.SetOriginalValue();

            return promotionProductModel;
        }

        private static List<RewardDescription> GetListRewardDescription(CommonProducts product)
        {
            var promotions = new List<RewardDescription>();
            var rewards = _promotionEngine.Evaluate(product.ContentLink);

            promotions.AddRange(rewards);
            if (!rewards.Any())
            {
                var variants = product.GetVariants();
                foreach (var variant in variants)
                {
                    rewards = _promotionEngine.Evaluate(variant);
                    if (rewards.Any())
                    {
                        promotions.AddRange(rewards);
                    }
                }
            }

            return promotions;
        }

        public static List<string> IndexCampaignProduct(this CommonProducts product)
        {
            var result = new List<string>(); 
            var now = DateTime.UtcNow;
            // neeed  cached
            var campaigns = _client.Search<SalesCampaign>()
                .Filter(s => !s.ValidFrom.GreaterThanNow() & s.ValidUntil.GreaterThanNow())
                .GetContentResult();

            foreach (var campaign in campaigns)
            {
                //Get all promotion for each campaign
                // neeed  cached
                var promotions = _promotionEngine.GetPromotionItemsForCampaign(campaign.ContentLink);

                // try second way to get item affect by promotions

                //var first = promotions.FirstOrDefault().Condition;
                //var type = contentLoader.GetOriginalType();
                //var ss = contentLoader.GetItems(first.Items, System.Globalization.CultureInfo.CurrentCulture);
                
                //var tmp = contentLoader.TryGet<ProductVariation>(first.Items.FirstOrDefault(), out var productVa1);
                //var tmp1 = contentLoader.TryGet<TestCategory>(first.Items.ToList()[1], out var productVa2);

                //Get promotions apply for this product
                var appliedPromotions = GetListRewardDescription(product);

                //Join 2 list to check if product is in this campaign to index campaign name
                var results = promotions.Join(appliedPromotions, a => a.Promotion.ContentLink, b => b.Promotion.ContentLink, (a, b) => a);
                if(results.Any())
                {
                    result.Add(campaign.Name);
                }
            }
            return result;
        }
      
        public static string IndexCategoriesProduct(this CommonProducts product)
        {
            var result = new List<string>();
            var now = DateTime.UtcNow;
            var parentNodes = product.GetCategories();

            foreach (var node in parentNodes)
            {
                // neeed  cached
                var category = contentLoader.Get<NodeContent>(node);
                if(category != null)
                {
                    return category.Code;
                }
            }
            return string.Empty;
        }
    }
}