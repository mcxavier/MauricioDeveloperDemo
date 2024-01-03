using System.Collections.Generic;

using Core.Domains.Catalogs.Filters;

namespace Core.Domains.Catalogs.Queries
{
    public class ShowcaseListingFilterRequest
    {
        public List<string> Filters { get; set; }

        public List<int> Categories { get; set; }

        public bool OnlyOnStock { get; set; } = true;

        public string Term { get; set; }

        public decimal? PriceFrom { get; set; }

        public decimal? PriceTo { get; set; }

        public string OrderBy { get; set; }

        public string OrderDirection { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public ProductFilter Convert()
        {
            return new ProductFilter
            {
                OnlyOnStock = this.OnlyOnStock,
                Filters = this.Filters,
                CategoriesIds = this.Categories,
                Term = this.Term,
                PriceRange = new ProductFilterPriceRange { From = this.PriceFrom, To = this.PriceTo },
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                OrderBy = this.OrderBy,
                OrderDirection = this.OrderDirection,
            };
        }
    }
}