using EPiServer.Find.ClientConventions;
using MyAlloySite.Commerce.Products;
using MyAlloySite.Extensions;

namespace MyAlloySite.Find
{
    public static class CommerceFindConvention
    {
        public static TypeConventionBuilder<CommonProducts> ApplyFieldConventions(this TypeConventionBuilder<CommonProducts> builder)
        {
            builder
                .IncludeField(s => s.IndexPromotion())
                .IncludeField(s => s.IndexCampaignProduct())
                .IncludeField(s => s.IndexCategoriesProduct());

            return builder;
        }
    }
}