using MyAlloySite.Api;
using MyAlloySite.Models.Pages;
using MyAlloySite.Models.ViewModels;
using System.Collections.Generic;

namespace MyAlloySite.DTO
{
    public class PromotionViewModel : PaginationModel
    {
        public string CampaignName { get; set; }

        public List<ProductDTOModel> Products { get; set; }

        public List<ProductDTOModel> Categories { get; set; }

        public List<SortModel> SortItems { get; set; }

        public IPageViewModel<SitePageData> CurrentPage { get; set; }
    }
}
