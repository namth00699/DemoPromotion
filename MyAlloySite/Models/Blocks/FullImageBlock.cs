using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAlloySite.Models.Blocks
{
    [SiteContentType(
       GroupName = Global.GroupNames.Specialized,
       GUID = "64ecd938-f378-439d-9551-7e213be9dc5a")]
    [SiteImageUrl]
    public class FullImageBlock : SiteBlockData
    {
        [Display(
           GroupName = SystemTabNames.Content,
           Order = 1
           )]
        [CultureSpecific]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}