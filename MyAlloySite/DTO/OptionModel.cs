using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAlloySite.DTO
{
    public class OptionModel
    {
        public int FilterType { get; set; }

        public string FilterAttribute { get; set; }

        public string DisplayOption { get; set; }
    }
}