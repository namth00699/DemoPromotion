using Electrolux.Core.Helpers.MVCHelper;
using Electrolux.Web.Features.ContentListingCarousel.Models.ViewModels;
using EPiServer.Find;
using EPiServer.ServiceLocation;
using MyAlloySite.Commerce.Products;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using MyAlloySite.Service;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyAlloySite.Api
{
    [RoutePrefix("api/content")]
    public class ProductApiController : ApiController
    {
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        private readonly IBuildQueryService sortingService = ServiceLocator.Current.GetInstance<IBuildQueryService>();
        public ProductApiController()
        {
        }

        [HttpPost]
        [Route("products")]
        public HttpResponseMessage GetProductByQuery(ProductRequestModel model)
        {
            HttpResponseMessage response;
            var query = _client.Search<CommonProducts>()
                .Filter(s => s.IndexCampaignProduct().Match(model.Campaign));
            var filter = _client.BuildFilter<CommonProducts>();

            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                filter = filter.And(x => x.IndexCategoriesProduct().Match(model.Category));
            }

            query = query.Filter(filter);

            query = sortingService.ApplySorting(model.Sort, query);
            query = sortingService.SetPageSize<PromotionPage>(null, query, model.PageSize, model.PageIndex);
            var searchResult = query.Select(s => new ProductDTOModel
            {
                Name = s.Name,
                Code = s.Code,
                Description = s.Description,
                Image = s.Thumbnail,
                ActualPrice = s.IndexPromotion().ActualPrice,
                OriginalPrice = s.IndexPromotion().OriginalPrice,
                Percent = s.IndexPromotion().GreatestPercent
            }).GetResult();

            var hasMore = model.PageIndex * model.PageSize < searchResult.TotalMatching;

            var viewRederer = new ViewRenderer();
            var html = viewRederer.RenderPartialViewToString("~/Views/PromotionPage/ProductListing.cshtml", searchResult.ToList());
            response = Request.CreateResponse(HttpStatusCode.OK, new ContentListingRespone() { Html = html, HasMore = hasMore });
            return response;
        }
    }
}
