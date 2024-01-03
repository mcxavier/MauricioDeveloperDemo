namespace Core.Dtos
{
    public class ProductInstagramDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Availability { get; set; }
        public string BasePrice { get; set; }
        public string Condition { get; set; } = "Novo";
        public string Link { get; set; }
        public string Image_link { get; set; }
        public string BrandName { get; set; }
        public string Additional_image_link { get; set; }
        public string Age_group { get; set; }
        public string Color { get; set; }
        public string Gender { get; set; }
        public string Item_group_id { get; set; }
        public string Fb_product_category { get; set; }
        public string Product_type { get; set; }
        public string Sale_price { get; set; }
        public string Sale_price_effective_date { get; set; }
        public string Size { get; set; }
        public string Visibility { get; set; }
        public int Inventory { get; set; }
        public string Material { get; set; }
        
    }
}