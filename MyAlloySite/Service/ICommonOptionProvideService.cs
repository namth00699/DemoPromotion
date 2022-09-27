using EPiServer.ServiceLocation;
using MyAlloySite.Constant;
using MyAlloySite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlloySite.Service
{
    interface ICommonOptionProvideService
    {
         List<SortModel> GetSortingOption();
    }

    [ServiceConfiguration(typeof(ICommonOptionProvideService))]
    public class CommonOptionProvideService : ICommonOptionProvideService
    {
        public List<SortModel> GetSortingOption()
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
