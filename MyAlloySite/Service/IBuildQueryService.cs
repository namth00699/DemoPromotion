using EPiServer.Find;
using EPiServer.ServiceLocation;
using MyAlloySite.Api;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Constant;
using MyAlloySite.DTO;
using MyAlloySite.Extensions;
using MyAlloySite.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAlloySite.Service
{
    public interface IBuildQueryService
    {
        ITypeSearch<CommonProducts> ApplySorting(string sort, ITypeSearch<CommonProducts> query);

        ITypeSearch<CommonProducts> SetPageSize(ITypeSearch<CommonProducts> query, int pageSize = 6, int pageIndex = 1); 

        ITypeSearch<CommonProducts> ApplyFilter(ProductRequestModel request, ITypeSearch<CommonProducts> query, IClient _client);

        ITypeSearch<CommonProducts> ApplyFacet(ITypeSearch<CommonProducts> query, PromotionPage promotionPage);
    }

    [ServiceConfiguration(ServiceType = typeof(IBuildQueryService))]
    public class SortingService : IBuildQueryService
    {
        public ITypeSearch<CommonProducts> ApplySorting(string sort, ITypeSearch<CommonProducts> query)
        {
            var dictionary = new Dictionary<string, Action>
            {
                {
                    GlobalValues.Default, () =>
                    {
                        query = query.OrderByDescending(s => s.IndexPromotion().GreatestPercent).OrderByDescending(s => s.IndexPromotion().ActualPrice);
                    }
                },
                {
                    GlobalValues.NameAsc, () =>
                    {
                        query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.Name);
                    }
                },
                {
                    GlobalValues.NameDes, () =>
                    {
                        query = query.OrderByDescending(s => s.DisplayName).OrderByDescending(s => s.Name);
                    }
                },
                {
                    GlobalValues.PercentAsc, () =>
                    {
                        query = query.OrderBy(s => s.IndexPromotion().GreatestPercent).OrderBy(s => s.IndexPromotion().ActualPrice);
                    }
                },
                 {
                    GlobalValues.PercentDesc, () =>
                    {
                        query = query.OrderByDescending(s => s.IndexPromotion().GreatestPercent).OrderByDescending(s => s.IndexPromotion().ActualPrice);
                    }
                },
                  {
                    GlobalValues.PriceAsc, () =>
                    {
                        query = query.OrderBy(s => s.IndexPromotion().ActualPrice).OrderByDescending(s => s.IndexPromotion().OriginalPrice);
                    }
                },
                   {
                    GlobalValues.PriceDesc, () =>
                    {
                        query = query.OrderByDescending(s => s.IndexPromotion().ActualPrice).OrderByDescending(s => s.IndexPromotion().ActualPrice);
                    }
                },
            };

            var isExist = dictionary.TryGetValue(sort, out var result);
            if (isExist)
            {
                result.Invoke();
            }
            else
            {
                query = query.OrderBy(s => s.DisplayName).ThenBy(s => s.Name);
                result.Invoke();
            }

            return query;
        }

        public ITypeSearch<CommonProducts> SetPageSize(ITypeSearch<CommonProducts> query, int pageSize = 6, int pageIndex = 1)
        {
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public ITypeSearch<CommonProducts> ApplyFilter(ProductRequestModel request, ITypeSearch<CommonProducts> query, IClient _client)
        {
            var filterProduct = _client.BuildFilter<CommonProducts>();
            
            if (!string.IsNullOrEmpty(request.Campaign))
            {
                filterProduct = filterProduct.And(s => s.IndexCampaignProduct().Match(request.Campaign));
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                filterProduct = filterProduct.And(s => s.IndexCategoriesProduct().Match(request.Category));
            }

            if (request.Filters != null && request.Filters.Any())
            {
                var filterPromotion = _client.BuildFilter<CommonProducts>();
                filterPromotion = BuildFilterFromFacets(request.Filters, filterPromotion);
                query = query.Filter(filterPromotion);
            }

            query = query.Filter(filterProduct);
            return query;
        }

        private FilterBuilder<CommonProducts> BuildFilterFromFacets(List<OptionModel> filters, FilterBuilder<CommonProducts> filterProduct)
        {
            int i = 1;
            foreach(var filter in filters)
            {
                if(filter.FilterType == (int)Constant.Constants.FilterType.PromotionType)
                {
                    var isParse = int.TryParse(filter.FilterAttribute, out var filterValue);
                    if(isParse && i == 1)
                    {
                        filterProduct = filterProduct.And(s => s.IndexPromotion().PromotionType.Match(filterValue));
                    }
                    else
                    {
                        filterProduct = filterProduct.Or(s => s.IndexPromotion().PromotionType.Match(filterValue));
                    }
                    i++;
                }
            }
            return filterProduct;
        }

        public ITypeSearch<CommonProducts> ApplyFacet(ITypeSearch<CommonProducts> query, PromotionPage promotion)
        {
            query = query.TermsFacetFor(s => s.IndexCategoriesProduct());
            if(promotion != null && promotion.DisplayFilterPromotionType)
            {
                query = query.FilterFacet(Constants.PromotionType.BuyItemsGetGifts.ToString(),
                    s => s.IndexPromotion().PromotionType.Match((int)Constants.PromotionType.BuyItemsGetGifts));
                query = query.FilterFacet(Constants.PromotionType.SaleOff.ToString(),
                    s => s.IndexPromotion().PromotionType.Match((int)Constants.PromotionType.SaleOff));
            }
            return query;
        }
    }
}
