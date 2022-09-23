using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electrolux.Web.Features.ContentListingCarousel.Models.ViewModels
{
    public class ContentListingRespone
    {
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }
    }
}