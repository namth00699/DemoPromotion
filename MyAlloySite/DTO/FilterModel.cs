using System.Collections.Generic;

namespace MyAlloySite.DTO
{
    public class FilterModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<OptionModel> Options { get; set; }
    }
}