//using EPiServer.Commerce.Marketing;
//using EPiServer.Framework.Localization;
//using EPiServer.ServiceLocation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace MyAlloySite.Promotions
//{
//    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton)]
//    public class BuyItemsGetGiftsProcessor : EntryPromotionProcessorBase<BuyItemsGetGifts>
//    {
//        private readonly CollectionTargetEvaluator _targetEvaluator;
//        private readonly FulfillmentEvaluator _fulfillmentEvaluator;
//        private readonly LocalizationService _localizationService;

//        public BuyItemsGetGiftsProcessor(
//              RedemptionDescriptionFactory redemptionDescriptionFactory,
//              CollectionTargetEvaluator targetEvaluator,
//              FulfillmentEvaluator fulfillmentEvaluator,
//              LocalizationService localizationService) 
//            : base(redemptionDescriptionFactory)
//        {
//            _targetEvaluator = targetEvaluator;
//            _fulfillmentEvaluator = fulfillmentEvaluator;
//            _localizationService = localizationService;
//        }
//        protected override RewardDescription Evaluate(BuyItemsGetGifts promotionData, PromotionProcessorContext context)
//        {
//            var lineItems = GetLineItems(context.OrderForm);
//            var condition = promotionData.Condition;
//            var applicableCodes = _targetEvaluator.GetApplicableCodes(lineItems, condition.Items, condition.MatchRecursive);
//            var fulfillmentStatus = _fulfillmentEvaluator.GetStatusForBuyQuantityPromotion(
//                applicableCodes,
//                lineItems,
//                condition.RequiredQuantity,
//                condition.PartiallyFulfilledThreshold);
//            var affectedEntries = context.EntryPrices.ExtractEntries(applicableCodes, condition.RequiredQuantity);
//            return RewardDescription.CreateGiftItemsReward(
//                fulfillmentStatus,
//                GetRedemptions(applicableCodes, promotionData, context),
//                promotionData,
//                promotionData.PercentageDiscount,
//                fulfillmentStatus.GetRewardDescriptionText(_localizationService));
//        }

//        protected override PromotionItems GetPromotionItems(BuyItemsGetGiftsProcessor promotionData)
//        {
//            var specificItems = new CatalogItemSelection(
//                promotionData.Condition.Items,
//                CatalogItemSelectionType.Specific,
//                promotionData.Condition.MatchRecursive);
//            return new PromotionItems(promotionData, specificItems, specificItems);
//        }

//        protected override bool CanBeFulfilled(BuyItemsGetGiftsProcessor promotionData, PromotionProcessorContext context)
//        {
//            if (promotionData.PercentageDiscount <= 0)
//            {
//                return false;
//            }
//            var lineItems = GetLineItems(context.OrderForm);
//            if (!lineItems.Any())
//            {
//                return false;
//            }
//            return true;
//        }

//        private IEnumerable<RedemptionDescription> GetRedemptions(IEnumerable<string> applicableCodes, BuyItemsGetGiftsProcessor promotionData, PromotionProcessorContext context)
//        {
//            var redemptions = new List<RedemptionDescription>();
//            var requiredQuantity = promotionData.Condition.RequiredQuantity;
//            var maxRedemptions = GetMaxRedemptions(promotionData.RedemptionLimits);
//            for (int i = 0; i < maxRedemptions; i++)
//            {
//                var affectedEntries = context.EntryPrices.ExtractEntries(applicableCodes, requiredQuantity);
//                if (affectedEntries == null)
//                {
//                    break;
//                }
//                redemptions.Add(CreateRedemptionDescription(affectedEntries));
//            }
//            return redemptions;
//        }
//    }
//}