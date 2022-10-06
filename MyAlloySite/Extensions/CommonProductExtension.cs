using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Promotions;
using MyAlloySite.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.Extensions
{
    public static class CommonProductExtension
    {
        private static readonly IPromotionEngine _promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();
        private static readonly IContentLoader contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private static readonly IContentTypeRepository contentRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
        private static readonly IEluxCache _eluxCache = ServiceLocator.Current.GetInstance<IEluxCache>(); 
        private static readonly IPromotionHelpService _promotionHelpService = ServiceLocator.Current.GetInstance<IPromotionHelpService>();

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
                PromotionType = _promotionHelpService.GetListPromotionType(promotions)
            };

            return _promotionHelpService.SetOriginalValue(promotionProductModel);
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
            var buildCacheKey = _eluxCache.BuildCacheKey(
                new Dictionary<string, object>
                {
                    { "campaign", nameof(SalesCampaign)},
                    { "language", ContentLanguage.PreferredCulture?.Name },
                }, string.Empty, string.Empty);

            var campaigns = _eluxCache.Get<List<SalesCampaign>>(buildCacheKey);

            if(campaigns == null)
            {
                var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();
                var type = contentRepository.Load<SalesCampaign>();
                var usages = contentModelUsage.ListContentOfContentType(type);
                campaigns = usages.Select(x => contentLoader.Get<SalesCampaign>(x.ContentLink)).ToList();
                _eluxCache.Add(buildCacheKey, campaigns, TimeSpan.FromHours(2), new[] { CacheMesterKeySpec.Categories.Campaign });
            }

            foreach (var campaign in campaigns)
            {
                //Get all promotion for each campaign
                var buildPromotionKey = _eluxCache.BuildCacheKey(
                   new Dictionary<string, object>
                   {
                        { "campaign", campaign},
                        { "promotion", nameof(PromotionItems)},
                        { "language", ContentLanguage.PreferredCulture?.Name },
                   }, string.Empty, string.Empty);

                var promotions = _eluxCache.Get<List<PromotionItems>>(buildPromotionKey);
                if (promotions == null)
                {
                    promotions = _promotionEngine.GetPromotionItemsForCampaign(campaign.ContentLink).ToList();
                    _eluxCache.Add(buildPromotionKey, promotions, TimeSpan.FromHours(2), new[] { CacheMesterKeySpec.Categories.Promotion });
                }
                
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