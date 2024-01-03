using System.Collections.Generic;
using Core.Models.Core.Products;

namespace Api.ViewModels
{

    public class ProductListingQueryModel
    {
        public Product Product { get; set; }
        public ProductDto ProductDto { get; set; }
        public IList<ProductSpecificationDto> Specifications { get; set; }
        public IEnumerable<object> Variations { get; set; }
    }

    public class ProductSpecificationDto
    {
        public int? TypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool Available { get; set; }
        public int Stock { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string Reference { get; set; }
        public string CommonReference { get; set; }
        public string Ncm { get; set; }
        public string OriginId { get; set; }
        public string Origin { get; set; }
        public string LinxUx { get; set; }
        public string PrincipalImage { get; set; }
        public decimal? BasePrice { get; set; }
        public IList<ProductSpecificationDto> Specifications { get; set; }

        public static ProductDto Convert(Product product)
        {
            return new ProductDto
            {
                Description = product.Description,
                Name = product.Name,
                Id = product.Id,
                Ncm = product.Ncm,
                Origin = product.Ncm,
                Reference = product.Reference,
                BrandName = product.BrandName,
                CommonReference = product.CommonReference,
                OriginId = product.OriginId,
                LinxUx = ""
            };
        }
    }
}