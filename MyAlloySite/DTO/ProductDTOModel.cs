using EPiServer.Core;
using MyAlloySite.Api;
using MyAlloySite.Models.ViewModels;
using System.Collections.Generic;

namespace MyAlloySite.DTO
{
    public class ProductDTOModel
    {
        public string Name { get; set; }

        public ContentReference Image { get; set; }

        public string Code { get; set; }

        public decimal? ActualPrice { get; set; }

        public decimal OriginalPrice { get; set; }

        public string Description { get; set; }

        public decimal Percent { get; set; }
    }

    public class PromotionDTOModel : PaginationModel
    {
        public string CampaignName { get; set; }

        public List<ProductDTOModel> Products { get; set; }

        public List<ProductDTOModel> Categories { get; set; }

        public List<SortModel> SortItems { get; set; }
    }
}