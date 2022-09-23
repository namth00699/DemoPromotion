using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Commerce.Marketing;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using MyAlloySite.Extensions.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyAlloySite.Models.Pages
{
    [SiteContentType(DisplayName = "PromotionPage", 
        GUID = "d9d1b1c3-3727-4b5c-920c-e409426e3fae")]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    public class PromotionPage : StandardPage
    {
        /*
                [CultureSpecific]
                [Display(
                    Name = "Main body",
                    Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual XhtmlString MainBody { get; set; }
         */

        [Display(
        Name = "Banner Image",
        GroupName = Global.TabNames.Banner,
        Order = 30)]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [Display(
        Name = "Campaign",
        Order = 30)]
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(SalesCampaign)})]
        public virtual ContentReference Campaign { get; set; }

        [Display(
           Name = "Setting Filter Price",
           Description = "",
           GroupName = Global.TabNames.Filter,
           Order = 40)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<FilterMoneyModel>))]
        public virtual IList<FilterMoneyModel> FilterPrice { get; set; }

        [Display(
          Name = "Setting PageSize",
          Description = "",
          GroupName = Global.TabNames.Filter,
          Order = 40)]
        [CultureSpecific]
        public virtual int PageSize { get; set; }
    }
}