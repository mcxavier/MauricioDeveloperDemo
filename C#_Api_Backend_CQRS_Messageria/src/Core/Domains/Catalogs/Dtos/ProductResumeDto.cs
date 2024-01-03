namespace Core.Dtos
{
    public class ProductResumeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal? BasePrice { get; set; }
    }
}