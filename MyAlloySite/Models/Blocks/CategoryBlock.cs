using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using MyAlloySite.Commerce.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAlloySite.Models.Blocks
{
    [SiteContentType(
       GroupName = Global.GroupNames.Specialized,
       GUID = "9ad4d5f4-4aff-4ad9-becb-e344b011ad83")]
    [SiteImageUrl]
    public class CategoryBlock : SiteBlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 330)]
        [CultureSpecific]
        [AllowedTypes(typeof(TestCategory))]
        public virtual IList<ContentReference> Categories { get; set; }
    }
}