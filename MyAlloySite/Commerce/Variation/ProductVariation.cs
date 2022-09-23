using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;

namespace MyAlloySite.Commerce.Variation
{
    [CatalogContentType(DisplayName = "Product Variation",
       GUID = "56743783-65ff-4c72-983d-bc12762c88c5",
       Description = "Electrolux Product Variation",
       MetaClassName = "ProductVariation")]
    public class ProductVariation : VariationContent
    {
    }
}