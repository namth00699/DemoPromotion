using EPiServer;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using MyAlloySite.Commerce.Category;
using MyAlloySite.DTO;
using MyAlloySite.Models.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyAlloySite.Controllers.Blocks
{
    public class CategoryBlockController : BlockController<CategoryBlock>
    {
        // GET: CategoryBlock
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        public CategoryBlockController()
        {
        }

        public override ActionResult Index(CategoryBlock currentBlock)
        {
            var categories = _contentLoader.GetItems(currentBlock.Categories, ContentLanguage.PreferredCulture);
            var result = new List<ProductDTOModel>();
            foreach(var item in categories)
            {
                var tmp = item as TestCategory;
                result.Add(new ProductDTOModel { Name = tmp.Name, Code = tmp.Code, Image = tmp.Image });
            }
            return PartialView(result);
        }
    }
}