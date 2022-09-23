using EPiServer.Find;
using EPiServer.ServiceLocation;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Constant;
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

        ITypeSearch<CommonProducts> SetPageSize<T>(T currentPage, ITypeSearch<CommonProducts> query, int pageSize = 6, int pageIndex = 1) where T : PromotionPage;

        int GetPageSize<T>(T currentPage) where T : PromotionPage;
    }

    [ServiceConfiguration(ServiceType = typeof(IBuildQueryService))]
    public class SortingService : IBuildQueryService
    {
        public ITypeSearch<CommonProducts> ApplySorting(string sort, ITypeSearch<CommonProducts> query)
        {
            var dictionary = new Dictionary<string, Action>
            {
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
            }

            return query;
        }

        public ITypeSearch<CommonProducts> SetPageSize<T>(T currentPage, ITypeSearch<CommonProducts> query, int pageSize = 6, int pageIndex = 1) where T : PromotionPage
        {
            if (currentPage != null)
            {
                pageSize = GetPageSize<T>(currentPage);
            }
            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public int GetPageSize<T>(T currentPage) where T : PromotionPage
        {
            return currentPage.PageSize > 0 ? currentPage.PageSize : 6;
        }
    }
}
