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
using MyAlloySite.ViewModel;
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
        private readonly IPromotionViewModelFactory _promotionViewModelFactory = ServiceLocator.Current.GetInstance<IPromotionViewModelFactory>();
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
                    var searchResults = _promotionViewModelFactory.Create(null, model);
                    results = new ProductResponseModel { HasMore = searchResults.Paging.HasMore, ProductDTOModels = searchResults.Products };
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
