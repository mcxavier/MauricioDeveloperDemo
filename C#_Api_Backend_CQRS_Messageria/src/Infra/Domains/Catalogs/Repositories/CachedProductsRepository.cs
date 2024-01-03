using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Filters;
using Core.Domains.Catalogs.Repositories;
using Core.Dtos;
using Core.Models.Core.Products;
using Core.SeedWork;
using Dapper;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.Repositories.Sql;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Infra.Extensions;

namespace Infra.Repositories
{
    public class CachedProductsRepository : IProductsRepository
    {
        private readonly CoreContext _context;
        private readonly SmartSalesIdentity _identity;
        private readonly IProductCache _productCache;

        public CachedProductsRepository(CoreContext context, SmartSalesIdentity identity, IProductCache productCache)
        {
            this._identity = identity;
            this._context = context;
            this._productCache = productCache;
        }

        private IQueryable<Product> query()
        {
            return _context.Products.Where(x => x.IsActive)
                           .Select(product => new Product
                           {
                               Id = product.Id,
                               Ncm = product.Ncm,
                               Name = product.Name,
                               Reference = product.Reference,
                               CommonReference = product.CommonReference,
                               Description = product.Description,
                               BrandName = product.BrandName,
                               Categories = product.Categories,
                               Variations = product.Variations.Where(x => x.IsActive).Select(variation => new ProductVariation
                               {
                                   Id = variation.Id,
                                   Name = variation.Name,
                                   ProductId = variation.Id,
                                   ImageUrl = variation.ImageUrl,
                                   BasePrice = variation.BasePrice,
                                   CostPrice = variation.CostPrice,
                                   ListPrice = variation.ListPrice,
                                   CompleteName = variation.CompleteName,
                                   StockKeepingUnit = variation.StockKeepingUnit,
                                   Images = variation.Images.Select(image => new ProductImage
                                   {
                                       Name = image.Name,
                                       UrlImage = image.UrlImage,
                                       IsPrincipal = image.IsPrincipal,
                                       ProductVariationId = image.ProductVariationId,
                                   }).ToList(),
                                   Specifications = variation.Specifications.Select(spec => new ProductSpecification
                                   {
                                       ProductVariationId = spec.ProductVariationId,
                                       Name = spec.Name,
                                       Description = spec.Description,
                                       Value = spec.Value,
                                       IsFilter = spec.IsFilter,
                                       TypeId = spec.TypeId,
                                   }).ToList()
                               }).ToList()
                           }).OrderBy(x => x.Id);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = _productCache.GetProductById(this._context, id);
            if (product == null)
            {
                product = await query().FirstOrDefaultAsync(x => x.Id == id);
                _productCache.UpdateProductById(this._context, id, product);
                return product;
            }
            else
            {
                return product;
            }
        }
        public Product GetProductById(int id)
        {
            return GetProductByIdAsync(id).Result;
        }

        public async Task<IPagedList<ProductResumeDto>> GetProductResumeByTerm(ProductFilter productFilter)
        {
            var parameters = new DynamicParameters(new
            {
                term = !string.IsNullOrEmpty(productFilter.Term) ? $"%{productFilter.Term}%" : null,
                pageIndex = productFilter.PageIndex,
                pageSize = productFilter.PageSize,
                categoryId = productFilter.CategoryId,
                categoriesIds = productFilter.CategoriesIds
            });

            var totalCount = 0;

            IEnumerable<ProductResumeDto> products;

            if (productFilter.Filters != null && productFilter.Filters.Any() && productFilter.Filters.Find(x => x.Equals("hasNext")).Any())
            {
                products = await (this._context.Database.GetDbConnection().QueryAsync<ProductResumeDto>(
                    ProductSqls.OptimizedSearchProductsSql(true),
                    param: parameters
                ));
            }
            else
            {
                IEnumerable<int> countList = await (this._context.Database.GetDbConnection().QueryAsync<int>(
                    ProductSqls.OptimizedSearchCountProductsSql(),
                    param: parameters
                ));

                products = await (this._context.Database.GetDbConnection().QueryAsync<ProductResumeDto>(
                    ProductSqls.OptimizedSearchProductsSql(),
                    param: parameters
                ));

                totalCount = countList.FirstOrDefault();
            }


            var pager = new StaticPagedList<ProductResumeDto>(products.ToList(), productFilter.PageIndex, productFilter.PageSize, totalCount);

            return pager;
        }

        public async Task<IPagedList<Product>> GetProductsByIds(IList<int> ids, int pageIndex = 1, int pageSize = 10)
        {

            return await Task.FromResult(query().Where(x => ids.Contains(x.Id)).ToPagedList(pageIndex, pageSize));
        }

        public async Task<IPagedList<Product>> GetProductsPaginated(int pageIndex = 1, int pageSize = 10)
        {

            return await Task.FromResult(query().ToPagedList(pageIndex, pageSize));
        }

        public async Task<IPagedList<ProductDto>> GetProductsFiltered(ProductFilter productFilter)
        {

            var lookupProducts = new Dictionary<int, ProductDto>();
            var lookupVariations = new Dictionary<int, VariationDto>();
            var lookupImages = new Dictionary<int, ProductImage>();
            var lookupSpecs = new Dictionary<int, ProductSpecification>();

            var parameters = new DynamicParameters(new
            {
                term = !string.IsNullOrEmpty(productFilter.Term) ? $"%{productFilter.Term}%" : null,
                containsFilters = productFilter.Filters?.Count > 0 ? "" : null,
                containsCategories = productFilter.CategoriesIds?.Count > 0 ? "" : null,
                filters = productFilter.Filters,
                categoriesIds = productFilter.CategoriesIds,
                priceTo = productFilter.PriceRange.To,
                priceFrom = productFilter.PriceRange.From,
                onlyOnStock = productFilter.OnlyOnStock,
                pageIndex = productFilter.PageIndex,
                pageSize = productFilter.PageSize,
                storeCode = this._identity.CurrentStoreCode
            });

            var totalCount = 0;
            this._context
                .Database
                .GetDbConnection()
                .Query<ProductDto, VariationDto, ProductImage, ProductSpecification, int, Product>(

                    ProductSqls.ProductsFilteredSql(productFilter.GetOrderBy(), productFilter.GetOrderDirection()),

                    (product, variation, image, spec, count) =>
                    {
                        totalCount = count;

                        if (!lookupProducts.TryGetValue(product.Id, out var productEntry))
                        {
                            lookupProducts.Add(product.Id, productEntry = product);
                        }

                        if (!lookupVariations.TryGetValue(variation.Id, out var variationEntry))
                        {
                            lookupVariations.Add(variation.Id, variationEntry = variation);
                            variationEntry.Units = variation.Units;

                            productEntry.VariationsDtos ??= new List<VariationDto>();
                            productEntry.VariationsDtos.Add(variationEntry);
                        }

                        if (!lookupImages.TryGetValue(image.Id, out var imageEntry))
                        {
                            lookupImages.Add(image.Id, imageEntry = image);

                            variationEntry.Images ??= new List<ProductImage>();
                            variationEntry.Images.Add(imageEntry);
                        }

                        if (!lookupSpecs.TryGetValue(spec.Id, out var specEntry))
                        {
                            lookupSpecs.Add(spec.Id, specEntry = spec);

                            variationEntry.Specifications ??= new List<ProductSpecification>();
                            variationEntry.Specifications.Add(specEntry);
                        }

                        return productEntry;
                    },
                    splitOn: "Id, Id, Id, Count",
                    param: parameters,
                    commandTimeout: 0
                );

            var pager = new StaticPagedList<ProductDto>(lookupProducts.Values.ToList(), productFilter.PageIndex, productFilter.PageSize, totalCount);

            return pager;
        }

        public async Task<IPagedList<ProductDto>> GetOptimizedProductsFiltered(ProductFilter productFilter)
        {

            var parameters = new DynamicParameters(new
            {
                term = !string.IsNullOrEmpty(productFilter.Term) ? $"%{productFilter.Term}%" : null,
                containsFilters = productFilter.Filters?.Count > 0 ? "" : null,
                containsCategories = productFilter.CategoriesIds?.Count > 0 ? "" : null,
                filters = productFilter.Filters,
                categoriesIds = productFilter.CategoriesIds,
                priceTo = productFilter.PriceRange.To,
                priceFrom = productFilter.PriceRange.From,
                onlyOnStock = productFilter.OnlyOnStock,
                pageIndex = productFilter.PageIndex,
                pageSize = productFilter.PageSize,
                storeCode = this._identity.CurrentStoreCode
            });

            var totalCount = 0;

            IEnumerable<int> productsIds = await this._context
                .Database
                .GetDbConnection()
                .QueryAsync<int>(
                    ProductSqls.OptimizedProductsFilteredSql(productFilter.GetOrderBy(), productFilter.GetOrderDirection(), productFilter),
                    param: parameters,
                    commandTimeout: 0
                );

            totalCount = productsIds.Count();

            IList<ProductDto> products = productsIds
                .Skip((productFilter.PageIndex - 1) * productFilter.PageSize)
                .Take(productFilter.PageSize)
                .Select(x =>
                {
                    Product product = GetProductById(x);
                    decimal basePrice = decimal.MaxValue;
                    IList<VariationDto> variations = product.Variations.Select(v =>
                    {
                        basePrice = Math.Min(v.BasePrice ?? 0, basePrice);
                        return new VariationDto
                        {
                            StockKeepingUnit = v.StockKeepingUnit,
                            ImageUrl = v.ImageUrl,
                            Name = v.Name,
                            CompleteName = v.CompleteName,
                            BasePrice = v.BasePrice.HasValue ? v.BasePrice : 0,
                            ListPrice = v.ListPrice.HasValue ? v.ListPrice : 0,
                            CostPrice = v.CostPrice.HasValue ? v.CostPrice : 0,
                            ProductId = v.ProductId,
                            Product = v.Product,
                            OriginId = v.OriginId,
                            Origin = v.Origin,
                            Images = v.Images,
                            Specifications = v.Specifications,
                            Units = 0
                        };
                    }).ToList();

                    return new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        BrandName = product.BrandName,
                        Reference = product.Reference,
                        CommonReference = product.CommonReference,
                        Ncm = product.Ncm,
                        OriginId = product.OriginId,
                        Origin = product.Origin,
                        Categories = product.Categories,
                        Variations = product.Variations,
                        VariationsDtos = variations,
                        BasePrice = basePrice
                    };
                }).ToList();

            foreach (var product in products)
            {
                foreach (var sku in product.VariationsDtos)
                {
                    sku.Units = _context.Stock.FirstOrDefault(x => x.StoreCode == this._identity.CurrentStoreCode && x.StockKeepingUnit == sku.StockKeepingUnit)?.Units ?? 0;
                }
            }

            IPagedList<ProductDto> pager = new StaticPagedList<ProductDto>(products, productFilter.PageIndex, productFilter.PageSize, totalCount);

            return pager;
        }

        public Task<ProductVariation> GetVariationByStockKeepingUnitAsync(string sku)
        {
            return this._context
                       .ProductVariations
                       .Include(x => x.Images)
                       .Include(x => x.Specifications)
                       .FirstOrDefaultAsync(x => x.StockKeepingUnit == sku);
        }
    }
}