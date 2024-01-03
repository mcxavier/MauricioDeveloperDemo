using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Infra.Extensions;
using System;
using Utils;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationProductHandler : IRequestHandler<PublishApplicationProduct, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationProductHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly IProductCache _productCache;

        public PublishApplicationProductHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationProductHandler> logger, IProductCache productCache)
        {
            this._context = context;
            this._identity = identity;
            this._logger = logger;
            this._productCache = productCache;
        }

        public async Task<Response> Handle(PublishApplicationProduct request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Skus == null || request.Skus.Count == 0 || request.Skus[0] == null)
                    return new Response("Produto descartado porque está sem SKU", true, request);

                var product = _context.Products.Where(x => x.OriginId == request.ProductId).FirstOrDefault();
                if (product == null)
                {
                    product = new Product();
                    product.Name = request.Title;
                    product.Description = request.Description;
                    product.BrandName = request.Brand.Name;
                    product.Reference = null;
                    product.CommonReference = null;
                    product.Ncm = null;
                    product.OriginId = request.ProductId;
                    product.Origin = null;
                    product.IsActive = request.Enabled;
                    product.CreatedAt = DateTimeBrazil.Now;
                    product.ModifiedAt = request.UpdatedAt;
                    product.ModifiedBy = "Linx.IO";
                    product.CreatedBy = "Linx.IO";

                    _context.Products.Add(product);
                    _context.SaveChanges();
                }
                else
                {
                    product.Name = request.Title;
                    product.Description = request.Description;
                    product.BrandName = request.Brand.Name;
                    product.Reference = null;
                    product.CommonReference = null;
                    product.Ncm = null;
                    product.OriginId = request.ProductId;
                    product.Origin = null;
                    product.IsActive = request.Enabled;
                    product.ModifiedAt = request.UpdatedAt;
                    product.ModifiedBy = "Linx.IO";
                }

                product.Variations = _context.ProductVariations.Where(x => x.ProductId == product.Id).ToList();
                product.Categories = _context.ProductCategories.Where(x => x.ProductId == product.Id).ToList();

                foreach (var sku in request.Skus)
                {
                    var variation = _context.ProductVariations.Where(x => x.StockKeepingUnit == sku.Sku && x.OriginId == sku.SkuId).FirstOrDefault();
                    if (variation == null)
                    {
                        variation = new ProductVariation()
                        {
                            Name = sku.Title,
                            OriginId = sku.SkuId,
                            StockKeepingUnit = sku.Sku,
                            CompleteName = sku.Title,
                            Product = product,
                            IsActive = sku.Enabled,
                            CreatedAt = DateTimeBrazil.Now,
                            CreatedBy = "Linx.IO",
                            ModifiedAt = request.UpdatedAt,
                            ModifiedBy = "Linx.IO"
                        };

                        _context.ProductVariations.Add(variation);
                    }
                    else
                    {
                        variation.Name = sku.Title;
                        variation.OriginId = sku.SkuId;
                        variation.CompleteName = sku.Title;
                        variation.StockKeepingUnit = sku.Sku;
                        variation.Product = product;
                        variation.IsActive = sku.Enabled;
                        variation.ModifiedAt = request.UpdatedAt;
                        variation.ModifiedBy = "Linx.IO";

                        _context.ProductVariations.Update(variation);
                    }

                    variation.Specifications = _context.ProductSpecifications.Where(x => x.ProductVariationId == variation.Id).ToList();
                    foreach (var variationOption in sku.VariationOptions)
                    {
                        var specification = _context.ProductSpecifications.Where(x => x.ProductVariationId == variation.Id && x.Name == variationOption.Name).FirstOrDefault();
                        if (specification == null)
                        {
                            specification = new ProductSpecification()
                            {
                                Name = variationOption.Name,
                                Description = variationOption.Data.Value,
                                Value = variationOption.Data.Value,
                                TypeId = ProductSpecificationType.FromString(variationOption.Id),
                                ProductVariationId = variation.Id,
                                ProductVariation = variation
                            };

                            _context.ProductSpecifications.Add(specification);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            specification.Value = variationOption.Data.Value;
                            specification.TypeId = ProductSpecificationType.FromString(variationOption.Id);
                            specification.Description = variationOption.Data.Value;
                            specification.ProductVariation = variation;

                            _context.ProductSpecifications.Update(specification);
                            await _context.SaveChangesAsync();
                        }

                        if (!variation.Specifications.Contains(specification))
                        {
                            variation.Specifications.Add(specification);
                        }
                    }

                    if (!product.Variations.Contains(variation))
                    {
                        product.Variations.Add(variation);
                    }
                }

                var child = request.Category;
                while (child != null)
                {
                    var category = _context.Categories.Where(x => x.Name == child.Name && x.OriginId == child.Id).FirstOrDefault();
                    if (category == null)
                    {
                        category = new Category()
                        {
                            Name = child.Name,
                            Description = child.Name,
                            OriginId = child.Id
                        };

                        _context.Categories.Add(category);

                        await _context.SaveChangesAsync();
                    }

                    var productCategory = _context.ProductCategories.Where(x => x.ProductId == product.Id && x.CategoryId == category.Id).FirstOrDefault();
                    if (productCategory == null)
                    {
                        productCategory = new ProductCategory()
                        {
                            CategoryId = category.Id,
                            ProductId = product.Id,
                            Product = product,
                            Category = category
                        };
                        _context.ProductCategories.Add(productCategory);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        productCategory.Category = category;
                        productCategory.Product = product;

                        _context.ProductCategories.Update(productCategory);
                        await _context.SaveChangesAsync();
                    }

                    child = child.Child;
                    bool productCategory_notFound = true;

                    foreach (ProductCategory pc in product.Categories)
                    {
                        if ((pc.CategoryId == productCategory.CategoryId) && (pc.ProductId == productCategory.ProductId))
                        {
                            productCategory_notFound = false;
                        }
                    }

                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    if (productCategory_notFound)
                    {
                        product.Categories.Add(productCategory);
                        await _context.SaveChangesAsync();
                    }
                }

                if (request.Images != null)
                {
                    foreach (var image in request.Images)
                    {
                        foreach (var skuId in image.SkuIds)
                        {
                            var productVariation = _context.ProductVariations.Where(x => x.StockKeepingUnit == skuId).FirstOrDefault();
                            if (productVariation != null)
                            {
                                var productImage = _context.ProductImages.Where(x => x.ProductVariationId == productVariation.Id && x.Name == image.Id).FirstOrDefault();
                                if (productImage == null)
                                {
                                    productImage = new ProductImage()
                                    {
                                        Name = image.Id,
                                        UrlImage = image.Url,
                                        IsPrincipal = false,
                                        ProductVariationId = productVariation.Id,
                                        ProductVariation = productVariation
                                    };

                                    _context.ProductImages.Add(productImage);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    productImage.UrlImage = image.Url;
                                    productImage.ProductVariation = productVariation;

                                    _context.ProductImages.Update(productImage);
                                    await _context.SaveChangesAsync();
                                }
                                if (!productVariation.Images.Contains(productImage))
                                {
                                    productVariation.Images.Add(productImage);
                                }
                            }
                        }
                    }

                }

                await _context.SaveChangesAsync();

                this._productCache.RemoveProductById(_context, product.Id, true);

                return new Response("Produto cadastrado", false);
            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true, request);
            }
        }
    }
}
