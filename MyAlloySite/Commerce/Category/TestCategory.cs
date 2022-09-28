using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace MyAlloySite.Commerce.Category
{
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,svg")]
    public class TestCategory : NodeContent
    {
        [Display(
        Name = "Banner Image",
        GroupName = Global.TabNames.Common,
        Order = 5)]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }

        [Display(GroupName = Global.TabNames.Common, Order = 1)]
        [CultureSpecific]
        public virtual bool DisplayFilter { get; set; }
    }
}