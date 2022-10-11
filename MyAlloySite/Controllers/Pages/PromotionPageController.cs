using EPiServer;
using EPiServer.Commerce.Marketing;
using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Cms;
using EPiServer.Globalization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using MyAlloySite.Api;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Category;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Constant;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using MyAlloySite.Service;
using MyAlloySite.ViewModel;
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
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly IBuildQueryService _buildQueryService = ServiceLocator.Current.GetInstance<IBuildQueryService>();
        private readonly IEluxCache _eluxCache = ServiceLocator.Current.GetInstance<IEluxCache>();
        private readonly IPromotionViewModelFactory _promotionViewModelFactory = ServiceLocator.Current.GetInstance<IPromotionViewModelFactory>();
        private static readonly ILogger Logger = LogManager.GetLogger();

        public PromotionPageController()
        {
        }
        public ActionResult Index(PromotionPage currentPage)
        {
            if(currentPage.Campaign == null)
            {
                return RedirectToAction("Index", "StartPage"); 
            }
            try
            {
                var buildCacheKey = _eluxCache.BuildCacheKey(
                       new Dictionary<string, object>
                       {
                            { "language", ContentLanguage.PreferredCulture?.Name },
                            { "campaign", currentPage.Campaign.ID }
                       }, nameof(PromotionPageController), nameof(Index));

                var cachedModel = _eluxCache.Get<PromotionViewModel>(buildCacheKey);
                if (cachedModel == null)
                {
                    cachedModel = _promotionViewModelFactory.Create(currentPage, new ProductRequestModel() { Campaign = string.Empty, Sort = GlobalValues.PercentDesc });
                    _eluxCache.Add(buildCacheKey, cachedModel, TimeSpan.FromHours(24), new[] { CacheMesterKeySpec.Categories.Promotion, CacheMesterKeySpec.Categories.Campaign });
                }

                return View(cachedModel);
            }
            catch (Exception ex)
            {
                Logger.Error("[ERROR] Failed at: {0}.{1}. Full Exception: {2}", "PromotionPageController", "Index", ex);
                return RedirectToAction("Index", "StartPage");
            }
        }


    }
}