using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.ServiceLocation;
using MyAlloySite.Api;
using MyAlloySite.Commerce.Products;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using MyAlloySite.Service;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.ViewModel
{
    [ServiceConfiguration(typeof(IPromotionViewModelFactory))]
    public class PromotionViewModelFactory : IPromotionViewModelFactory
    {
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ICommonOptionProvideService _optionProvide = ServiceLocator.Current.GetInstance<ICommonOptionProvideService>();
        private readonly IBuildQueryService _buildQueryService = ServiceLocator.Current.GetInstance<IBuildQueryService>();
        private readonly ICategoryService _categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();

        public PromotionViewModel Create(PromotionPage currentPage, ProductRequestModel request)
        {
            var promotionModel = new PromotionViewModel(currentPage);
            if (currentPage != null)
            {
                request.PageSize = currentPage.PageSize;
                request.Campaign = currentPage != null ? _contentLoader.Get<SalesCampaign>(currentPage.Campaign).Name : string.Empty;
            }
            var products = Search(currentPage, request);

            if (products != null)
            {
                var convert = products as SearchResults<ProductDTOModel>;
                promotionModel.Categories = GetFacets(convert);
                promotionModel.Products = products.ToList();
                promotionModel.CampaignName = request.Campaign;
                promotionModel.SortItems = _optionProvide.GetSortingOption();
                promotionModel.Paging = new PaginationModel
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    HasMore = request.PageIndex * request.PageSize < convert.TotalMatching
                };
            }

            return promotionModel;
        }

        private List<ProductDTOModel> GetFacets(SearchResults<ProductDTOModel> products)
        {
            var termCategories = products.Facets.FirstOrDefault() as TermsFacet;
            var resultCategories = termCategories.Terms.Select(s => new ProductDTOModel { Code = s.Term }).ToList();
            return resultCategories;
        }

        private IEnumerable<ProductDTOModel> Search(PromotionPage currentPage, ProductRequestModel model)
        {
            var query = _client.Search<CommonProducts>();

            query = _buildQueryService.ApplyFilter(model, query, _client);
            query = _buildQueryService.ApplyFacet(query);
            query = _buildQueryService.ApplyFilter(model, query, _client);
            query = _buildQueryService.ApplySorting(model.Sort, query);
            query = _buildQueryService.SetPageSize(query, model.PageSize, model.PageIndex);

            return query.Select(s => new ProductDTOModel
            {
                Name = s.Name,
                Code = s.Code,
                Description = s.Description,
                Image = s.Thumbnail,
                ActualPrice = s.IndexPromotion().ActualPrice,
                OriginalPrice = s.IndexPromotion().OriginalPrice,
                Percent = s.IndexPromotion().GreatestPercent
            }).GetResult();
        }
    }
}