using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using MyAlloySite.Commerce.Category;
using MyAlloySite.Commerce.Products;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using MyAlloySite.Models.ViewModels;
using MyAlloySite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAlloySite.Controllers.Pages
{
    public class PromotionPageController : PageController<PromotionPage>
    {
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        private readonly IPromotionEngine promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();
        private readonly IContentLoader contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly IBuildQueryService buildQueryService = ServiceLocator.Current.GetInstance<IBuildQueryService>();

        public PromotionPageController()
        {
        }
        public ActionResult Index(PromotionPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);
            /* Implementation of action. You can create your own view model class that you pass to the view or
             * you can pass the page type for simpler templates */
            //var pages = _client.Search<CommonProducts>().Take(10).GetResult();
            var now = DateTime.UtcNow;
            var resultProducts = new List<ProductDTOModel>();
            var resultCategories = new List<ProductDTOModel>();
            var campaignName = string.Empty;

            if (currentPage.Campaign != null)
            {
                var currCampaign = contentLoader.Get<SalesCampaign>(currentPage.Campaign);
                campaignName = currCampaign.Name;
                //var campaigns = _client.Search<SalesCampaign>()
                //.Filter(s => !s.ValidFrom.GreaterThanNow() & s.ValidUntil.GreaterThanNow() & s.Name.Match(currCampaign.Name))
                //.GetContentResult();

                //var query = _client.Search<CommonProducts>()
                //    .Filter(s => s.IndexCampaignProduct().Match(currCampaign.Name));

                //if (currentPage.FilterPrice != null)
                //{
                //    var priceFilter = _client.BuildFilter<CommonProducts>();
                //    priceFilter = priceFilter.And(x => x.IndexPromotion().ActualPrice.GreaterThan(0));
                //    priceFilter = priceFilter.And(x => x.IndexPromotion().ActualPrice.LessThan(1000));
                //    query = query.Filter(priceFilter);
                //}

                //query = query.OrderByDescending(s => s.IndexPromotion().GreatestPercent)
                //    .ThenByDescending(s => s.IndexPromotion().SavedAmount);

                //SetPageSize(currentPage, query);

                //var searchResult = query.Select(s => new ProductDTOModel
                //{
                //    Name = s.Name,
                //    Code = s.Code,
                //    Description = s.Description,
                //    Image = s.Thumbnail,
                //    ActualPrice = s.IndexPromotion().ActualPrice,
                //    OriginalPrice = s.IndexPromotion().OriginalPrice,
                //    Percent = s.IndexPromotion().GreatestPercent
                //}).GetResult();

                //results.AddRange(searchResult);

                var query = _client.MultiSearch<ProductDTOModel>();
                var filterProduct = _client.BuildFilter<CommonProducts>();
                filterProduct = filterProduct.And(s => s.IndexCampaignProduct().Match(currCampaign.Name));

                var filterCategory = _client.BuildFilter<TestCategory>();
                filterCategory = filterCategory.And(s => !s.Code.Match(string.Empty));
                filterCategory = filterCategory.And(s => s.DisplayFilter.Match(true));

                var queryProduct = _client.Search<CommonProducts>()
                    .Filter(filterProduct);
                queryProduct = queryProduct.TermsFacetFor(s => s.IndexCategoriesProduct());
                queryProduct = buildQueryService.ApplySorting(string.Empty, queryProduct);
                queryProduct = buildQueryService.SetPageSize(currentPage, queryProduct);

                query.Search<CommonProducts, ProductDTOModel>(z => queryProduct
                    .Select(s => new ProductDTOModel
                    {
                        Name = s.Name,
                        Code = s.Code,
                        Description = s.Description,
                        Image = s.Thumbnail,
                        ActualPrice = s.IndexPromotion().ActualPrice,
                        OriginalPrice = s.IndexPromotion().OriginalPrice,
                        Percent = s.IndexPromotion().GreatestPercent
                    }))
                .Search<TestCategory, ProductDTOModel>(s => s.Filter(filterCategory)
                     .Select(t => new ProductDTOModel { Name = t.Name, Code = t.Code, Image = t.Image }));
                // need cached result
                var test = query.GetResult();
                if (test != null)
                {
                    var list = test.ToList();
                    resultProducts = list[0].ToList();

                    var termCategories = list[0].Facets.FirstOrDefault() as TermsFacet;
                    resultCategories = list[1].Join(termCategories.Terms , a => a.Code, b => b.Term, (a, b) => a ).ToList();
                }
            }

            return View(new PromotionDTOModel
            {
                Products = resultProducts,
                Categories = resultCategories,
                CampaignName = campaignName,
                SortItems = SortingExtension.GetSortingOption(),
                PageSize = buildQueryService.GetPageSize(currentPage)
            });
        }

      
    }
}