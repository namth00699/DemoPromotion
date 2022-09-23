using MyAlloySite.Constant;
using MyAlloySite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAlloySite.Extensions
{
    public static class SortingExtension
    {
        public static List<SortModel> GetSortingOption()
        {
            var result = GlobalValues.SortingDictionary.Select(s => new SortModel
            {
                Name = s.Key,
                Value = s.Value
            });
            return result.ToList();
        }
    }
}