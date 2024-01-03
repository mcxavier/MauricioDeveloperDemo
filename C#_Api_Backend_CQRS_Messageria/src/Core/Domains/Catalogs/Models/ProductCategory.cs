using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductCategory : Entity<int>
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}