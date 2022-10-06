using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MyAlloySite.Commerce.Category;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Commerce.Variation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAlloySite.Promotions
{
    [ContentType(DisplayName = "Buy Items Get A Free Gift",
       GUID = "ddcf4c92-1c6b-4bca-8c0f-bd1b84a3cf4c",
       Description = "Product B needs to be installed and bonus product C",
       GroupName = "Promotions")]
    public class BuyItemsGetGifts : EntryPromotion
    {
        [PromotionRegion("Condition")]
        [Display(Name = "Required of Buy Items", Order = 10)]
        [Range(1, 99)]
        public virtual int RequiredQuantity { get; set; }

        [DistinctList]
        [PromotionRegion("Condition")]
        [AllowedTypes(typeof(ProductVariation))]
        [Display(Name = "Qualifying Items", Order = 15)]
        public virtual IList<ContentReference> Items { get; set; }

        [PromotionRegion("Reward")]
        [AllowedTypes(typeof(ProductVariation))]
        [Display(Name = "Gift Item(s)", Order = 20)]
        public virtual IList<ContentReference> GiftItems { get; set; }
    }
}