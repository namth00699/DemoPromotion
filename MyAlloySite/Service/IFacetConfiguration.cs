using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAlloySite.Service
{
    public interface IFacetConfiguration
    {
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
