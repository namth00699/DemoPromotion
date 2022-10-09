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
        private static IContentLoader _contentLoader;

        public void Initialize(InitializationEngine context)
        {
            _catalogContentClientConventions = context.Locate.Advanced.GetInstance<ContentClientConventions>();
            _catalogContentClientConventions.ApplyConventions(SearchClient.Instance.Conventions);
            _contentEvents = context.Locate.Advanced.GetInstance<IContentEvents>();
            _contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();
            _contentEvents.PublishedContent += ContentEvents_PublishedContent;
        }

        private void ContentEvents_PublishedContent(object sender, ContentEventArgs e)
        {
            if (e.Content is EntryPromotion promotion)
            {
                try
                {
                    // Update the index of the parent product when updating the variant
                    List<CommonProducts> products = GetProductsAffectByPromotion(promotion);
                    ContentIndexer.Instance.Index(products);

                    PurgeProductListMemCache();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger().Error(ex.Message, ex);
                }
            }
        }

        private List<CommonProducts> GetProductsAffectByPromotion(EntryPromotion promotion)
        {
            var current = _contentLoader.Get<EntryPromotion>(promotion.ContentLink);
            var products = new List<CommonProducts>();
            if (current != null && current is BuyItemsGetGifts buyItemsGetGifts)
            {
                foreach (var item in buyItemsGetGifts.Items)
                {
                    var variationContent = _contentLoader.Get<VariationContent>(item);
                    var product = variationContent.GetParentProducts();
                    var commonProduct = _contentLoader.Get<CommonProducts>(product?.FirstOrDefault());
                    if (commonProduct != null)
                    {
                        products.Add(commonProduct);
                    }
                }
            }
            if (current != null && current is BuyFromCategoryGetItemDiscount saleOff)
            {
                var commonProducts = _contentLoader.GetChildren<CommonProducts>(saleOff.Category);
                products.AddRange(commonProducts);
            }

            return products;
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