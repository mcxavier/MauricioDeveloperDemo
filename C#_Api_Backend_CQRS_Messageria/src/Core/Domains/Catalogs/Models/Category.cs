using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginId { get; set; } 
    }
}