using System.Collections.Generic;
using Core.Models.Core.Products;

namespace Core.Dtos
{
    public class ProductDto : Product
    {
        public IList<VariationDto> VariationsDtos { get; set; }
        public decimal? BasePrice { get; set; }
    }

    public class VariationDto : ProductVariation
    {
        public int Units { get; set; }
    }
}