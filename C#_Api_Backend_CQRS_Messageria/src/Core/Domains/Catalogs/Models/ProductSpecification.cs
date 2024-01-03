using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductSpecification : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public bool IsFilter { get; set; }
        public int? ProductVariationId { get; set; }
        public ProductVariation ProductVariation { get; set; }
        public int? TypeId { get; set; }
        public ProductSpecificationType Type { get; set; }
    }
}