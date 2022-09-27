using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Category;
using MyAlloySite.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlloySite.Service
{
    interface ICategoryService
    {
        List<ProductDTOModel> GetAllDisplayCategory();
    }

    [ServiceConfiguration(typeof(ICategoryService))]
    public class ContentService : ICategoryService
    {
        private static readonly IEluxCache _eluxCache = ServiceLocator.Current.GetInstance<IEluxCache>();
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();

        public List<ProductDTOModel> GetAllDisplayCategory()
        {
            var buildCacheKey = _eluxCache.BuildCacheKey(
                new Dictionary<string, object>
                {
                    { "model", nameof(TestCategory)},
                    { "language", ContentLanguage.PreferredCulture?.Name },
                }, string.Empty, string.Empty);

            var items = _eluxCache.Get<List<ProductDTOModel>>(buildCacheKey);

            if (items == null)
            {
                var categories = _client.Search<TestCategory>()
                    .Filter(s => s.DisplayFilter.Match(true))
                    .Filter(s => !s.Code.Match(string.Empty))
                    .Select(t => new ProductDTOModel { Name = t.Name, Code = t.Code, Image = t.Image })
                    .GetResult();

                items = categories.ToList();
                _eluxCache.Add(buildCacheKey, categories, TimeSpan.FromHours(2), new[] { CacheMesterKeySpec.Categories.Campaign });
            }
            return items;
        }
    }
}
