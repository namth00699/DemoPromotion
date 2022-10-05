using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using MyAlloySite.Models.Blocks;
using MyAlloySite.Service;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyAlloySite.Controllers.Blocks
{
    public class CategoryBlockController : BlockController<CategoryBlock>
    {
        // GET: CategoryBlock
        private readonly ICategoryService _categoryService = ServiceLocator.Current.GetInstance<ICategoryService>();
        public CategoryBlockController()
        {
        }

        public override ActionResult Index(CategoryBlock currentBlock)
        {
            var myProperty = ControllerContext.ParentActionViewContext.ViewData["Category"];
            var results = _categoryService.GetDisplayCategory(myProperty, currentBlock);
            
            return PartialView(results);
        }
    }
}