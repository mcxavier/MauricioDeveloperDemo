using System.Collections.Generic;
using Core.SeedWork;

namespace Core.Domains.Catalogs.Filters
{

    public class ProductFilter : IOrderedFilter, IPagedFilter
    {
        public string Term { get; set; }
        public List<string> Filters { get; set; }
        public List<int>? CategoriesIds { get; set; }
        public int? CategoryId { get; set; }
        public bool OnlyOnStock { get; set; }
        public ProductFilterPriceRange PriceRange { get; set; } = new ProductFilterPriceRange();
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }

    public class ProductFilterPriceRange
    {
        public decimal? From { get; set; }
        public decimal? To { get; set; }
    }
}