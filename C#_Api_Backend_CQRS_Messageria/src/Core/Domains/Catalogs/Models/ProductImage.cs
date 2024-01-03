using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductImage : Entity<int>
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public bool IsPrincipal { get; set; }
        public int? ProductVariationId { get; set; }
        public ProductVariation ProductVariation { get; set; }
    }
}