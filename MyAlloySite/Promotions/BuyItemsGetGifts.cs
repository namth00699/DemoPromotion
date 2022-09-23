using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using MyAlloySite.Commerce.Category;
using MyAlloySite.Commerce.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAlloySite.Promotions
{
    [ContentType(GUID = "76EBFEFF-2CFB-42F2-B4A3-EA5EA5A41515")]
    public class BuyItemsGetGifts : EntryPromotion
    {
        [PromotionRegion("Condition")]
        [Display(Name = "Required of Buy Items", Order = 10)]
        [AllowedTypes(typeof(TestCategory))]
        public virtual IList<ContentReference> BuyItems { get; set; }

        [PromotionRegion("Condition")]
        [Display(Name = "Received Item", Order = 10)]
        [AllowedTypes(typeof(CommonProducts))]
        public virtual IList<ContentReference> Gifts { get; set; }

        [PromotionRegion("Condition")]
        [Display(Name = "Required of Buy Items", Order = 10)]
        [Range(1, 99)]
        public virtual int RequiredQuantity { get; set; }
    }
}