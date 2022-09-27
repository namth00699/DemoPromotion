using Electrolux.Core.Helpers.MVCHelper;
using Electrolux.Web.Features.ContentListingCarousel.Models.ViewModels;
using EPiServer.Find;
using EPiServer.Globalization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Products;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using MyAlloySite.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyAlloySite.Api
{
    [RoutePrefix("api/content")]
    public class ProductApiController : ApiController
    {
        private static readonly ILogger Logger = LogManager.GetLogger();
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        private readonly IBuildQueryService _sortingService = ServiceLocator.Current.GetInstance<IBuildQueryService>();
        private readonly IEluxCache _eluxCache = ServiceLocator.Current.GetInstance<IEluxCache>();
        public ProductApiController()
        {
        }

        [HttpPost]
        [Route("products")]
        public HttpResponseMessage GetProductByQuery(ProductRequestModel model)
        {
            HttpResponseMessage response;

            try
            {
                var cacheKey = _eluxCache.BuildCacheKey(
                        new Dictionary<string, object>()
                        {
                    { nameof(model), JsonConvert.SerializeObject(model) },
                    { "language", ContentLanguage.PreferredCulture?.Name }
                        },
                        nameof(ProductApiController),
                        nameof(GetProductByQuery));

                var results = _eluxCache.Get<ProductResponseModel>(cacheKey);
                if (results == null)
                {
                    var query = _client.Search<CommonProducts>()
                    .Filter(s => s.IndexCampaignProduct().Match(model.Campaign));
                    var filter = _client.BuildFilter<CommonProducts>();

                    if (!string.IsNullOrWhiteSpace(model.Category))
                    {
                        filter = filter.And(x => x.IndexCategoriesProduct().Match(model.Category));
                    }

                    query = query.Filter(filter);

                    query = _sortingService.ApplySorting(model.Sort, query);
                    query = _sortingService.SetPageSize<PromotionPage>(null, query, model.PageSize, model.PageIndex);
                    var searchResults = query.Select(s => new ProductDTOModel
                    {
                        Name = s.Name,
                        Code = s.Code,
                        Description = s.Description,
                        Image = s.Thumbnail,
                        ActualPrice = s.IndexPromotion().ActualPrice,
                        OriginalPrice = s.IndexPromotion().OriginalPrice,
                        Percent = s.IndexPromotion().GreatestPercent
                    }).GetResult();

                    var hasMore = model.PageIndex * model.PageSize < searchResults.TotalMatching;
                    results = new ProductResponseModel { HasMore = hasMore, ProductDTOModels = searchResults.ToList() };
                    _eluxCache.Add(cacheKey, results, TimeSpan.FromHours(24), new[] { CacheMesterKeySpec.Categories.Promotion });
                }


                var viewRederer = new ViewRenderer();
                var html = viewRederer.RenderPartialViewToString("~/Views/PromotionPage/ProductListing.cshtml", results.ProductDTOModels);
                response = Request.CreateResponse(HttpStatusCode.OK, new ContentListingRespone() { Html = html, HasMore = results.HasMore });
                return response;
            }
            catch (Exception e)
            {
                Logger.Error("[ERROR] Failed at: {0}.{1}. Full Exception: {2}", "ProductApiController", "GetProductByQuery", e);
                return response = Request.CreateResponse(HttpStatusCode.OK, e.InnerException);
            }
        }
    }
}
