using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace MyAlloySite.Commerce.Products
{
    public class CommonProducts : ProductContent
    {
        //[Display(GroupName = "EPiServerCMS_SettingsPanel", Order = 3)]
        //[CultureSpecific]
        //[Searchable]
        //public virtual string ProductCode { get; set; }

        [Display(GroupName = Global.TabNames.Banner, Order = 3)]
        [CultureSpecific]
        public virtual string Description { get; set; }

        [Display(
        Name = "Thumbnail",
        GroupName = Global.TabNames.Banner,
        Order = 4)]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Thumbnail { get; set; }
    }
}