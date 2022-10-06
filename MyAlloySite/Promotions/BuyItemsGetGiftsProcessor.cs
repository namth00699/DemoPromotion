using EPiServer.Commerce.Marketing;
using EPiServer.Core.Internal;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.Catalog.ContentTypes;
using MyAlloySite.Constant;
using MyAlloySite.Commerce.Variation;

namespace MyAlloySite.Promotions
{
    [ServiceConfiguration(Lifecycle = ServiceInstanceScope.Singleton)]
    public class BuyItemsGetAFreeGiftProcessor : EntryPromotionProcessorBase<BuyItemsGetGifts>
    {
        private readonly ContentLoader _contentLoader;
        private readonly GiftItemFactory _giftItemFactory;

        public BuyItemsGetAFreeGiftProcessor(RedemptionDescriptionFactory redemptionDescriptionFactory,
            ContentLoader contentLoader, GiftItemFactory giftItemFactory) :
            base(redemptionDescriptionFactory)
        {
            _contentLoader = contentLoader;
            _giftItemFactory = giftItemFactory;
        }

        protected override PromotionItems GetPromotionItems(BuyItemsGetGifts promotionData)
        {
            return new PromotionItems(promotionData,
                new CatalogItemSelection(promotionData.Items, CatalogItemSelectionType.Specific, true),
                new CatalogItemSelection(promotionData.GiftItems, CatalogItemSelectionType.Specific, true));
        }

        protected override RewardDescription Evaluate(BuyItemsGetGifts promotionData, PromotionProcessorContext context)
        {
            if (!context.OrderForm.Shipments.Any() ||
                !context.OrderForm.Shipments.SelectMany(x => x.LineItems).Any() ||
                promotionData.RequiredQuantity == 0 ||
                promotionData.Items == null ||
                !promotionData.Items.Any())
            {
                return NotFulfilledRewardDescription(promotionData, context, FulfillmentStatus.NotFulfilled);
            }

            var allLineItems = context.OrderForm.Shipments.SelectMany(x => x.LineItems);
            var currentPromotionItemsInCart = GetCurrentPromotionItemsInCart(promotionData, context.OrderForm);

            if (!currentPromotionItemsInCart.Any())
            {
                return NotFulfilledRewardDescription(promotionData, context, FulfillmentStatus.NotFulfilled);
            }

            var qualifyingItemsQuantity = allLineItems.Where(x => currentPromotionItemsInCart.Contains(x.Code)).Sum(x => x.Quantity);

            var numberOfGiftItemsToAdd = GetRedemptionLimits(promotionData, (int)qualifyingItemsQuantity / promotionData.RequiredQuantity);

            if (numberOfGiftItemsToAdd == decimal.Zero)
            {
                return NotFulfilledRewardDescription(promotionData, context, FulfillmentStatus.NotFulfilled);
            }

            var trmRedemptions = GetRedemptions(promotionData, context, numberOfGiftItemsToAdd);


            var result = RewardDescription.CreateGiftItemsReward(FulfillmentStatus.Fulfilled, trmRedemptions, promotionData, "fullfilled");

            if (result.Status.Equals(FulfillmentStatus.Fulfilled))
                SaveGiftInfoIntoProductLineItem(context.OrderGroup, promotionData.GiftItems.Select(x => _contentLoader.Get<VariationContent>(x)).Select(x => x.Code), currentPromotionItemsInCart);

            return result;
        }


        private void SaveGiftInfoIntoProductLineItem(IOrderGroup orderGroup, IEnumerable<string> giftItemVariantionCodes, IEnumerable<string> productCodes)
        {
            foreach (var productCode in productCodes)
            {
                var productLineItem = orderGroup.GetFirstForm().GetAllLineItems().FirstOrDefault(l =>
                    l.Code.Equals(productCode, StringComparison.InvariantCultureIgnoreCase));

                if (productLineItem == null)
                    continue;

                var giftCodeMetadata = productLineItem.Properties[Metadata.LineItem.FreeGiftItems]?.ToString();
                if (string.IsNullOrWhiteSpace(giftCodeMetadata))
                {
                    productLineItem.Properties[Metadata.LineItem.FreeGiftItems] = string.Join(",", giftItemVariantionCodes);
                }
                else
                {
                    foreach (var giftItemVariantionCode in giftItemVariantionCodes)
                    {
                        if (!giftCodeMetadata.Contains(giftItemVariantionCode))
                            giftCodeMetadata += $",{giftItemVariantionCode}";
                    }
                    productLineItem.Properties[Metadata.LineItem.FreeGiftItems] = giftCodeMetadata;
                }
            }
        }

        private IEnumerable<string> GetCurrentPromotionItemsInCart(BuyItemsGetGifts promotionData, IOrderForm orderForm)
        {
            var allQualifyingItemCodes = promotionData.Items.Select(x => _contentLoader.Get<VariationContent>(x)).Select(x => x.Code);
            var allLineItemCodes = orderForm.Shipments.SelectMany(x => x.LineItems).Select(x => x.Code);

            return allQualifyingItemCodes.Intersect(allLineItemCodes).ToList();
        }

        private int GetRedemptionLimits(BuyItemsGetGifts promotionData, int numberOfGiftItemsToAdd)
        {
            var value = numberOfGiftItemsToAdd;

            if (promotionData.RedemptionLimits.PerCustomer.HasValue &&
                promotionData.RedemptionLimits.PerCustomer < value)
            {
                value = promotionData.RedemptionLimits.PerCustomer.Value;
            }

            if (promotionData.RedemptionLimits.PerOrderForm.HasValue &&
                promotionData.RedemptionLimits.PerOrderForm < value)
            {
                value = promotionData.RedemptionLimits.PerOrderForm.Value;
            }

            if (promotionData.RedemptionLimits.PerPromotion.HasValue &&
                promotionData.RedemptionLimits.PerPromotion < value)
            {
                value = promotionData.RedemptionLimits.PerPromotion.Value;
            }

            return Math.Min(value, numberOfGiftItemsToAdd);
        }

        private IEnumerable<RedemptionDescription> GetRedemptions(BuyItemsGetGifts promotionData, PromotionProcessorContext context, decimal numberOfGiftItemsToAdd)
        {
            var redemptionDescriptionList = new List<RedemptionDescription>();

            var allGiftItems = promotionData.GiftItems.Select(x => _contentLoader.Get<ProductVariation>(x));

            if (!allGiftItems.Any()) return redemptionDescriptionList;

            var giftItems = _giftItemFactory.CreateGiftItems(allGiftItems.Select(x => x.ContentLink), context);
            for (var i = 0; i < numberOfGiftItemsToAdd; i++)
            {
                redemptionDescriptionList.Add(CreateRedemptionDescription(giftItems));
            }
            return redemptionDescriptionList;
        }

    }
}