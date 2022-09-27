using MyAlloySite.Api;
using MyAlloySite.DTO;
using MyAlloySite.Models.Pages;

namespace MyAlloySite.ViewModel
{
    interface IPromotionViewModelFactory
    {
        PromotionViewModel Create(PromotionPage currentPage, ProductRequestModel request);
    }
}
