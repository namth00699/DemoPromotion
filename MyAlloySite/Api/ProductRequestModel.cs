namespace MyAlloySite.Api
{
    public class ProductRequestModel : PaginationModel
    {
        public string Category { get; set; }

        public string Campaign { get; set; }

        public string Sort { get; set; }
    }
}