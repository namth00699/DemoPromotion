using MyAlloySite.Facet;
using System.Collections.Generic;

namespace MyAlloySite.Facets
{
    public class FacetGroupOption
    {
        public string GroupName { get; set; }
        public List<FacetOption> Facets { get; set; }
        public string GroupFieldName { get; set; }
    }
}