using MyAlloySite.Api;
using MyAlloySite.Models.Pages;
using MyAlloySite.Models.ViewModels;
using System.Collections.Generic;

namespace MyAlloySite.DTO
{
    public class PromotionViewModel : PageViewModel<PromotionPage>
    {
        public PromotionViewModel(PromotionPage currentPage)
           : base(currentPage)
        {

        }

        public string CampaignName { get; set; }

        public List<ProductDTOModel> Products { get; set; }

        public List<ProductDTOModel> Categories { get; set; }

        public List<SortModel> SortItems { get; set; }

        public PaginationModel Paging { get; set; }
    }
}
