using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Marketing.Promotions;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Globalization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Find;
using MyAlloySite.Promotions;
using MyAlloySite.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.InitializeModule
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    [InitializableModule]
    public class FindConfigModule : IConfigurableModule
    {
        private readonly Injected<EPiServer.Find.IClient> client;
        private static ContentClientConventions _catalogContentClientConventions;
        private static IContentEvents _contentEvents;
        private static IPromotionHelpService _promotionHelpService;

        public void Initialize(InitializationEngine context)
        {
            _catalogContentClientConventions = context.Locate.Advanced.GetInstance<ContentClientConventions>();
            _catalogContentClientConventions.ApplyConventions(SearchClient.Instance.Conventions);
            _contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            _promotionHelpService = context.Locate.Advanced.GetInstance<IPromotionHelpService>();
            _contentEvents.PublishedContent += ContentEvents_PublishedContent;
        }

        private void ContentEvents_PublishedContent(object sender, ContentEventArgs e)
        {
            if (e.Content is EntryPromotion promotion)
            {
                try
                {
                    // Update the index of the parent product when updating the variant
                    List<CommonProducts> products = _promotionHelpService.GetProductsAffectByPromotion(promotion);
                    ContentIndexer.Instance.Index(products);

                    PurgeProductListMemCache();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger().Error(ex.Message, ex);
                }
            }
        }

       

        private void PurgeProductListMemCache()
        {
            var cache = ServiceLocator.Current.GetInstance<IEluxCache>();
            cache.Remove(CacheMesterKeySpec.Categories.Promotion);
        }

        public void Uninitialize(InitializationEngine context)
        {
            _contentEvents.PublishedContent -= ContentEvents_PublishedContent;
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<CatalogContentClientConventions, ContentClientConventions>();
        }
    }
}