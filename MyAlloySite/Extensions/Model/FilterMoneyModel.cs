using EPiServer.DataAnnotations;
using EPiServer.PlugIn;
using MyAlloySite.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyAlloySite.Extensions.Model
{
    [PropertyDefinitionTypePlugIn]
    public class FilterMoneyModelProperty : PropertyListBase<FilterMoneyModel>
    {

    }

    public class FilterMoneyModel
    {
        [CultureSpecific]
        [Display(
            Name = "Lower Range",
            GroupName = Global.TabNames.Filter,
            Order = 10)]
        [Range(0, int.MaxValue)]
        public virtual decimal Lower { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Upper Range",
            GroupName = Global.TabNames.Filter,
            Order = 20)]
        //[Range(1, int.MaxValue)]
        public virtual decimal Upper { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Currency",
            GroupName = Global.TabNames.Filter,
            Order = 20)]
        [Required]
        public virtual string Currency { get; set; }
    }
}