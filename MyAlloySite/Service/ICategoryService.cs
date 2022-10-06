using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using MyAlloySite.Cache;
using MyAlloySite.Commerce.Category;
using MyAlloySite.DTO;
using MyAlloySite.Models.Blocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlloySite.Service
{
    interface ICategoryService
    {
        List<ProductDTOModel> GetAllDisplayCategory();

        List<ProductDTOModel> GetDisplayCategory(object codes, CategoryBlock currentBlock);
    }

    [ServiceConfiguration(typeof(ICategoryService))]
    public class ContentService : ICategoryService
    {
        private static readonly IEluxCache _eluxCache = ServiceLocator.Current.GetInstance<IEluxCache>();
        private readonly IClient _client = ServiceLocator.Current.GetInstance<IClient>();
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

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

        public List<ProductDTOModel> GetDisplayCategory(object codes, CategoryBlock currentBlock)
        {
            var list= new List<string>();
            var canCast = ((IEnumerable)codes).Cast<object>()
                                   .Select(x => x == null ? x : x.ToString());
            if (canCast != null)
            {
                list = canCast.OfType<string>().ToList();
            }

            var categories = _contentLoader.GetItems(currentBlock.Categories, ContentLanguage.PreferredCulture);
            var results = new List<ProductDTOModel>();
           
            if (list == null || !list.Any())
            {
                foreach (var item in categories)
                {
                    var tmp = item as TestCategory;
                    results.Add(new ProductDTOModel { Name = tmp.Name, Code = tmp.Code, Image = tmp.Image });
                }
            }
            else
            {
                foreach (var item in categories)
                {
                    var tmp = item as TestCategory;
                    if(list.Contains(tmp.Code))
                    {
                        results.Add(new ProductDTOModel { Name = tmp.Name, Code = tmp.Code, Image = tmp.Image });
                    }
                }
            }
            return results;
        }
    }
}
