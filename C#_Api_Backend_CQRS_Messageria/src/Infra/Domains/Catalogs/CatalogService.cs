using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using Core.Domains.Catalogs.Dtos;
using Core.Domains.Catalogs.Queries;
using Core.Domains.Catalogs.Repositories;
using Core.Models.Core.Catalogs;
using Core.Models.Core.Ordering;
using Core.SharedKernel;
using Infra.EntitityConfigurations.Contexts;
using Infra.Responses;
using Infra.Domains.Catalogs.Responses;
using Infra.Extensions;
using Core.Models.Core.Products;
using Core.Domains.Catalogs.Filters;
using Core.Dtos;
using Core.Domains.Catalogs.Requests;
using Infra.ExternalServices.Authentication;

namespace Infra.Domains.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly CoreContext _context;
        private readonly IdentityContext _identityContext;
        private readonly IProductsRepository _productRepository;
        private readonly SmartSalesIdentity _identity;
        private readonly ICatalogCache _catalogResponseCache;
        public bool useMemoryCache = false;
        const int paidOrderStatus = (int)OrderStatusEnum.Paid;

        public CatalogService(CoreContext context, IdentityContext identityContext, ICatalogCache catalogResponseCache, IProductsRepository productRepository, SmartSalesIdentity identity)
        {
            _context = context;
            _identityContext = identityContext;
            _catalogResponseCache = catalogResponseCache;
            _productRepository = productRepository;
            _identity = identity;
        }

        public async Task<FilterCatalogResponse> GetCatalogsResume(FilterCatalogQuery catalogQuery, string storeCode)
        {

            DateTime now = DateTime.Now;

            catalogQuery.PageIndex = Math.Max(1, catalogQuery.PageIndex);

            var catalogFromStore = this._context.Catalogs.Where(x => x.StoreCode == storeCode
                                                                && (catalogQuery.Name == null
                                                                        || x.Name.ToLower().Contains(catalogQuery.Name.ToLower()))
                                                                && (catalogQuery.SellerId == null
                                                                        || x.SellerId == catalogQuery.SellerId)
                                                                && (catalogQuery.Status == 0 // Any status
                                                                        || (catalogQuery.Status == 1 && (x.ExpiresAt == null
                                                                                                        || x.ExpiresAt >= now)) // Active
                                                                        || (catalogQuery.Status == 2 && x.ExpiresAt < now) // Expired
                                                                    )
                                                                && (catalogQuery.BeginsAtInitialDate == null
                                                                        || x.BeginsAt >= catalogQuery.BeginsAtInitialDate)
                                                                && (catalogQuery.BeginsAtFinalDate == null
                                                                        || x.BeginsAt <= catalogQuery.BeginsAtFinalDate)
                                                                );

            var orderedCatalogFromStore = catalogQuery.OrderByCreatedAt ?
                                            catalogFromStore.OrderByDescending(x => x.CreatedAt)
                                            : catalogFromStore;
            var pagedCatalogs = orderedCatalogFromStore
            .Skip((catalogQuery.PageIndex - 1) * catalogQuery.PageSize)
            .Take(catalogQuery.PageSize)
            .Include("Products.Product")
            .Include("Customers.Customer")
            .ToList();
            List<Catalog> catalogsToUpdate = new List<Catalog>();

            pagedCatalogs.ForEach(catalog =>
            {
                bool hasNull = (catalog.NumOfPieces == null
                                || catalog.NumOfSales == null
                                || catalog.Revenues == null
                                || catalog.SentContacts == null);
                if (hasNull)
                {
                    if (catalog.NumOfSales == null || catalog.Revenues == null)
                    {

                        var orders = _context.Orders.Include(x => x.Status)
                                                    .Where(x => x.CatalogId == catalog.Id
                                                        && x.Status.Id == paidOrderStatus);
                        catalog.NumOfSales = catalog.NumOfSales ?? orders.Count();
                        catalog.Revenues = catalog.Revenues ?? orders.Sum(x => x.Value);
                    }
                    if (!catalog.Products.Any())
                    {
                        catalog.Products = _context.CatalogProducts
                                            .Where(x => x.CatalogId == catalog.Id).ToList();
                    }
                    catalog.NumOfPieces = catalog.NumOfPieces ?? catalog.Products.Count();
                    catalog.SentContacts = catalog.SentContacts ?? catalog.Customers
                                                                    .Where(x => x.SentAt.HasValue)
                                                                    .Count().ToString()
                                                                    + "/"
                                                                    + catalog.Customers.Count()
                                                                    .ToString();
                    catalogsToUpdate.Add(catalog);
                }
            });
            if (catalogsToUpdate.Any())
            {
                _context.Catalogs.UpdateRange(catalogsToUpdate);
                await _context.SaveChangesAsync();
            }

            int totalCount = catalogFromStore.Count();

            IEnumerable<CatalogResumeDto> filteredCatalogs = pagedCatalogs.Select(catalog =>
            {
                return new CatalogResumeDto
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    CreatedAt = catalog.CreatedAt == null ? null : catalog.CreatedAt.Value.ToLocalTime(),
                    BeginsAt = catalog.BeginsAt == null ? null : catalog.BeginsAt.Value.ToLocalTime(),
                    ExpiresAt = catalog.ExpiresAt == null ? null : catalog.ExpiresAt.Value.ToLocalTime(),
                    NumOfPieces = catalog.NumOfPieces,
                    NumOfSales = catalog.NumOfSales,
                    Revenues = catalog.Revenues,
                    SentContacts = catalog.SentContacts,
                    Status = now <= (catalog.ExpiresAt ?? now)
                            ? "Ativo" : "Expirado"
                };
            });


            double totalPagesDouble = (double)totalCount / (double)catalogQuery.PageSize;
            PagerInfo pagerInfo = new PagerInfo
            {
                CurrentPage = catalogQuery.PageIndex,
                PageSize = catalogQuery.PageSize,
                TotalPages = (int)Math.Ceiling(totalPagesDouble),
                TotalRows = totalCount
            };

            FilterCatalogResponse response = new FilterCatalogResponse
            {
                filteredCatalogs = filteredCatalogs,
                pagerInfo = pagerInfo
            };

            return response;
        }

        public async Task<CatalogDetailsResponse> GetCatalogDetailsById(int id)
        {
            if (useMemoryCache)
            {
                var res = _catalogResponseCache.GetCatalogDetailsById(id);
                if (res != null)
                {
                    return res;
                }
            }

            var catalogEntity = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == id);

            if (catalogEntity == null)
            {
                return null;
            }

            var orders = _context.Orders.Include(x => x.Status).Where(x => x.CatalogId == catalogEntity.Id && x.Status.Id == paidOrderStatus);
            if (orders != null)
            {
                // Calculate the Number of Sales and Revenues for this catalog details.
                catalogEntity.NumOfSales = catalogEntity.NumOfSales ?? orders.Count();
                catalogEntity.Revenues = catalogEntity.Revenues ?? orders.Sum(x => x.Value);
            }

            List<int> productsIds = await _context.CatalogProducts.Where(x => x.CatalogId == id).Select(x => x.ProductId).ToListAsync();

            List<CatalogDetailsResponse_Product> products = new List<CatalogDetailsResponse_Product>();
            List<CatalogProductListingResponse> catalogProducts = this.GetCatalogProductsById(id);

            products = catalogProducts.Select(product => new CatalogDetailsResponse_Product
            {
                Id = product.Product.Id,
                Name = product.Product.Name,
                Description = product.Product.Description,
                BasePrice = product.Variations.MinBy(v => v.BasePrice).Select(v => v.BasePrice).FirstOrDefault() ?? 0,
                ImageUrl = product.Variations.Select(v => v.Images).FirstOrDefault()?.Select(i => i.UrlImage).FirstOrDefault(),
                Reference = product.Product.Reference
            }).ToList();

            var sellers = _context.Sellers.Where(x => catalogEntity.SellerId == x.Id).ToList();

            CatalogDetailsResponse_Seller seller = sellers?.Select(seller =>
            {
                return new CatalogDetailsResponse_Seller
                {
                    Id = seller.Id,
                    Name = seller.Name
                };
            }).FirstOrDefault();

            List<CatalogCustomer> catalogCustomers = _context.CatalogCustomers.Where(x => catalogEntity.Id == x.CatalogId).ToList();

            List<int> customersIds = catalogCustomers.Select(x => x.CustomerId).ToList() ?? new List<int>();
            if (catalogEntity.Customers == null)
            {
                catalogEntity.Customers = catalogCustomers;
            }

            var customers = _context.Customers.Where(x => customersIds.Contains(x.Id)).ToList().Select(customer =>
                {
                    CatalogCustomer catalogCustomer = catalogCustomers.FirstOrDefault(x => customer.Id == x.CustomerId);
                    if (customer == null) return null;

                    return new CatalogDetailsResponse_Customer
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Phone = customer.Phone,
                        Received = catalogCustomer.SentAt.HasValue
                    };
                }).ToList();

            DateTime now = DateTime.Now;

            CatalogDetailsResponse catalogDetails = new CatalogDetailsResponse
            {
                Name = catalogEntity.Name,
                CreatedAt = catalogEntity.CreatedAt,
                BeginsAt = catalogEntity.BeginsAt,
                ExpiresAt = catalogEntity.ExpiresAt,
                Products = products,
                Seller = seller,
                Customers = customers,
                Revenues = catalogEntity.Revenues,
                Status = now <= (catalogEntity.ExpiresAt ?? now) ? "Ativo" : "Expirado"
            };

            if (useMemoryCache)
            {
                _catalogResponseCache.UpdateCatalogDetailsById(id, catalogDetails);
            }

            return catalogDetails;
        }

        public async Task<CatalogProductsResponse> GetCatalogById(int id)
        {
            var catalogEntity = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == id);

            if (catalogEntity == null)
            {
                return null;
            }

            //.Include(x => x.Products)
            //                                       .ThenInclude(x => x.Product)
            //                                       .ThenInclude(x => new Product { Id = x.Id })


            //.Include(x => x.Products)
            //    .ThenInclude(x => x.Product)
            //        .ThenInclude(x => x.Variations)
            //            .ThenInclude(x => x.Images)
            //.Include(x => x.Products)
            //    .ThenInclude(x => x.Product)
            //        .ThenInclude(x => x.Variations)
            //            .ThenInclude(x => x.Specifications)
            //                .ThenInclude(x => x.Type)
            List<int> productsIds = await _context.CatalogProducts.Where(x => x.CatalogId == id)
                                            .Select(x => x.ProductId).ToListAsync();
            List<Product> products = new List<Product>();
            products = _context.Products.Where(y => productsIds.Contains(y.Id)).ToList();
            //    .Include(x=> x.Variations).ThenInclude(x => x.Images)
            //    .Include(x=> x.Variations).ThenInclude(x => x.Specifications).ThenInclude(x => x.Type)
            //    .ToList();


            //var productsX = new ConcurrentBag<Product>();
            //Parallel.ForEach(productsIds, async x => 
            //{
            //    var a = await this._context.Products.FirstOrDefaultAsync(y => y.Id == x);
            //    productsX.Add(a);
            //});
            //products.ToList();
            //foreach (int x in productsIds)
            //{
            //    var a = this._context.Products.FirstOrDefault(y => y.Id == x);
            //    products.Add(a);
            //}

            CatalogProductsResponse catalog = new CatalogProductsResponse
            {
                Name = catalogEntity.Name,
                Products = products
            };

            return catalog;
        }

        public List<CatalogProductListingResponse> GetCatalogProductsById(int catalogId, out PagerInfo pagerInfo, int pageIndex, int pageSize)
        {
            var result = GetCatalogProductsById(catalogId);

            pagerInfo = new PagerInfo
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                TotalRows = result.Count,
                TotalPages = (int)Math.Ceiling((decimal)result.Count / (decimal)pageSize)
            };

            return result;
        }

        public List<CatalogProductListingResponse> GetCatalogProductsById(int catalogId)
        {
            var result = new List<CatalogProductListingResponse>();

            var productsIds = this._context.CatalogProducts.Where(x => x.CatalogId == catalogId).Select(x => x.ProductId).ToList();
            foreach (var item in productsIds)
            {
                var product = _productRepository.GetProductById(item);
                if (product != null)
                {
                    IList<ProductVariation> variations = product.Variations?.ToList() ?? new List<ProductVariation>();
                    var resultitem = new CatalogProductListingResponse
                    {
                        Product = product,
                        Variations = variations.Select(v => new VariationDto
                        {
                            Id = v.Id,
                            StockKeepingUnit = v.StockKeepingUnit,
                            ImageUrl = v.ImageUrl,
                            Name = v.Name,
                            CompleteName = v.CompleteName,
                            BasePrice = v.BasePrice,
                            ListPrice = v.ListPrice,
                            CostPrice = v.CostPrice,
                            ProductId = v.ProductId,
                            Product = v.Product,
                            Images = v.Images,
                            Specifications = v.Specifications,
                            Units = 0
                        }).ToList(),
                        Specifications = variations.SelectMany(x => x.Specifications).Where(x => x.TypeId == ProductSpecificationType.Color.Id || x.TypeId == ProductSpecificationType.Size.Id)
                                                   .Select(x => new ProductSpecificationDto
                                                   {
                                                       TypeId = x.TypeId,
                                                       Value = x.Value,
                                                       Name = x.Name,
                                                       Description = x.Description
                                                   }).DistinctBy(x => x.Value).ToList(),
                    };

                    foreach (var variation in resultitem.Variations)
                    {
                        variation.Units = _context.Stock.FirstOrDefault(x => x.StoreCode == this._identity.CurrentStoreCode && x.StockKeepingUnit == variation.StockKeepingUnit)?.Units ?? 0;
                    }

                    result.Add(resultitem);
                }
            }

            return result;
        }

        public async Task<Response> PostCatalogCustomerReceived(int ReceivedCatalogId, int ReceivedCustomerId)
        {
            var catalogEntity = await _context.Catalogs.FirstOrDefaultAsync(x => x.Id == ReceivedCatalogId);
            Response res = new Response();
            if (catalogEntity == null)
            {
                res.IsError = true;
                res.Message = "Catálogo não encontrado.";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(404, res.Message));
                return res;
            }

            var customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == ReceivedCustomerId);

            if (customerEntity == null)
            {
                res.IsError = true;
                res.Message = "Cliente não encontrado.";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(404, res.Message));
                return res;
            }

            var catalogCustomerEntity = await _context.CatalogCustomers.FirstOrDefaultAsync(x => x.CatalogId == ReceivedCatalogId && x.CustomerId == ReceivedCustomerId);

            if (catalogCustomerEntity == null)
            {
                res.IsError = true;
                res.Message = "Relação catálogo-cliente não encontrada.";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(404, res.Message));
                return res;
            }
            catalogCustomerEntity.SentAt = DateTime.Now;
            _context.CatalogCustomers.Update(catalogCustomerEntity);
            catalogEntity.SentContacts = null;

            _context.SaveChanges();

            _catalogResponseCache.RemoveCatalogDetailsById(ReceivedCatalogId);

            res.Message = "Relação catálogo-cliente atualizada com sucesso.";
            return res;
        }

        public async Task<Response> CreateCatalog(CreateCatalogRequest request, string storePortal, int? id = null)
        {
            var res = new Response();

            if (id != null) request.Id = id;

            if (storePortal == null)
            {
                res.IsError = true;
                res.Message = "Store portal não está presente no header.";
                res.Errors = new List<Error>
                {
                    new Error(422, res.Message)
                };

                return res;
            }

            var storeEntity = this._identityContext.Stores.FirstOrDefault(x => x.PortalUrl == storePortal);
            if (storeEntity == null)
            {
                // NotFound (404)
                res.IsError = true;
                res.Message = "Loja não encontrada.";
                res.Errors = new List<Error>
                {
                    new Error(404, res.Message)
                };
                return res;
            }

            string action_result = request.Id == null ? "criado" : "atualizado";
            DateTime? nullDateTime = null;
            Catalog catalog = null;
            var seller = request.SellerId.HasValue ? this._context.Sellers.FirstOrDefault(c => c.Id == request.SellerId) : new Core.Models.Core.Customers.Seller();

            if (request.Id == null)
            {
                catalog = new Catalog
                {
                    Name = request.Name,
                    SellerId = request.SellerId,
                    ExpiresAt = request.ExpiresAt == null ? nullDateTime : TimeZoneInfo.ConvertTime((DateTime)request.ExpiresAt, TimeZoneInfo.Utc),
                    BeginsAt = request.BeginsAt ?? DateTime.UtcNow,
                    CreatedBy = seller.Name,
                    ModifiedBy = seller.Name
                };

                this._context.Catalogs.Add(catalog);
                _context.SaveChanges();

                catalog.BeginsAt = catalog.CreatedAt;
                _context.SaveChanges();
            }
            else
            {
                catalog = this._context.Catalogs.FirstOrDefault(c => c.Id == request.Id);

                if (storeEntity.StoreCode == catalog.StoreCode)
                {
                    catalog.Name = request.Name ?? catalog.Name;
                    catalog.SellerId = request.SellerId;
                    catalog.ExpiresAt = request.ExpiresAt == null ? nullDateTime : TimeZoneInfo.ConvertTime((DateTime)request.ExpiresAt, TimeZoneInfo.Utc);
                    catalog.BeginsAt = request.BeginsAt == null ? catalog.BeginsAt : TimeZoneInfo.ConvertTime((DateTime)request.BeginsAt, TimeZoneInfo.Utc);
                    catalog.ModifiedBy = seller.Name;

                    this._context.Catalogs.Update(catalog);
                    _context.SaveChanges();
                }
                else
                {
                    res.IsError = true;
                    res.Message = "Loja não confere com a cadastrada no catálogo.";
                    res.Errors = new List<Error>
                    {
                        new Error(401, res.Message)
                    };

                    return res;
                }
            }

            var catalogProducts = new List<CatalogProduct>();
            if (request.ProductsIds != null)
            {
                catalogProducts = request.ProductsIds.Select(productId => new CatalogProduct
                {
                    ProductId = productId,
                    CatalogId = catalog.Id
                }).ToList();
            }

            var customers = new List<CatalogCustomer>();
            if (request.CustomersIds != null)
            {
                customers = request.CustomersIds.Select(customerId => new CatalogCustomer
                {
                    CustomerId = customerId,
                    CatalogId = catalog.Id
                }).ToList();
            }

            if (request.Id == null)
            {
                _context.AddRange(customers);
                _context.AddRange(catalogProducts);
            }
            else
            {
                var old_catalogCustomers = await _context.CatalogCustomers.Where(c => c.CatalogId == catalog.Id).ToListAsync();
                var old_catalogProducts = await _context.CatalogProducts.Where(c => c.CatalogId == catalog.Id).ToListAsync();

                if (old_catalogCustomers.Any()) _context.CatalogCustomers.RemoveRange(old_catalogCustomers);
                if (old_catalogProducts.Any()) _context.CatalogProducts.RemoveRange(old_catalogProducts);

                await _context.AddRangeAsync(customers);
                await _context.AddRangeAsync(catalogProducts);

                catalog.Customers = customers;
                catalog.Products = catalogProducts;
                catalog.NumOfPieces = catalogProducts.Count();
            }

            var success = _context.SaveChanges();

            if (success >= customers.Count() + catalogProducts.Count())
            {
                _catalogResponseCache.RemoveCatalogDetailsById(catalog.Id);
                _catalogResponseCache.RemoveCatalogProductsById(catalog.Id);
                await GetCatalogDetailsById(catalog.Id);

                res.Message = "Catálogo " + action_result + " com sucesso.";
                res.Payload = catalog.Id;
                return res;
            }
            else
            {
                throw new Exception("Falha na hora de salvar informações do catálogo.");
            }
        }

        public async Task<Response> GetCatalogProducts(string term, int pageIndex, int pageSize)
        {
            Response res = new Response();
            var filters = new ProductFilter
            {
                Term = term,
                PageSize = pageIndex,
                PageIndex = pageSize
            };

            var result = (await this._productRepository.GetProductsFiltered(filters)).ToList();

            var products = result.Select(x =>
            {
                var variation = x.VariationsDtos.FirstOrDefault() ?? new VariationDto();
                return new ProductResumeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    BrandName = x.BrandName,
                    Reference = x.Reference,
                    Description = x.Description,
                    ImageUrl = variation.ImageUrl,
                    BasePrice = variation.BasePrice
                };
            });


            if (products.Any())
            {
                res.Message = "Produtos";
                res.Payload = products;
                return res;
            }
            else
            {
                res.IsError = true;
                res.Message = "Catálogo não encontrado.";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(404, res.Message));
                return res;
            }
        }

        public async Task<Response> GetAllProducts(ShowcaseListingFilterRequest filter)
        {
            var res = new Response();

            var products = await this._productRepository.GetOptimizedProductsFiltered(filter.Convert());

            var listDtos = new List<CatalogProductListingResponse>();

            foreach (var product in products)
            {
                List<VariationDto> variations = product.VariationsDtos?.ToList() ?? new List<VariationDto>();

                var vw = new CatalogProductListingResponse
                {
                    ProductDto = CatalogProductListingResponse_ProductDto.Convert(product),
                    Variations = variations.Select(v => new VariationDto
                    {
                        Id = v.Id,
                        StockKeepingUnit = v.StockKeepingUnit,
                        ImageUrl = v.ImageUrl,
                        Name = v.Name,
                        CompleteName = v.CompleteName,
                        BasePrice = v.BasePrice,
                        ListPrice = v.ListPrice,
                        CostPrice = v.CostPrice,
                        ProductId = v.ProductId,
                        Product = v.Product,
                        Images = v.Images,
                        Specifications = v.Specifications,
                        Units = v.Units
                    }).OrderByDescending(x => x.Units).ToList()
                };

                vw.ProductDto.Specifications = variations.Where(x => x.BasePrice > 0).SelectMany(x => x.Specifications.Where(y => y.TypeId == ProductSpecificationType.Color.Id || y.TypeId == ProductSpecificationType.Size.Id),
                    (dto, result) => new ProductSpecificationDto
                    {
                        TypeId = result.TypeId,
                        Value = result.Value,
                        Name = result.Name,
                        Description = result.Description,
                        Available = dto.Units > 0
                    }).DistinctBy(x => x.Value).ToList();

                listDtos.Add(vw);
            }

            if (listDtos.Any())
            {
                res.Message = "Produtos";
                res.Payload = listDtos;
                res.SetPagerInfo(products);
                return res;
            }
            else
            {
                res.IsError = true;
                res.Message = "Produtos não encontrados.";
                res.Errors = new List<Error>
                {
                    new Error(404, res.Message)
                };
                return res;
            }
        }

        public async Task<Response> GetCatalogsItensInfos(int id)
        {
            Response res = new Response();
            List<ProductInstagramDto> instagramList = new List<ProductInstagramDto>();

            try
            {

                var productsIds = this._context
                                    .CatalogProducts
                                    .Where(x => x.CatalogId == id)
                                    .Select(x => x.ProductId);

                Catalog catalog = this._context
                                    .Catalogs
                                    .FirstOrDefault(x => x.Id == id);

                foreach (int productId in productsIds)
                {
                    Product product = _productRepository.GetProductById(productId);

                    if (product?.Variations != null)
                    {

                        foreach (ProductVariation variation in product?.Variations)
                        {
                            var color = product?.Variations?.SelectMany(x => x.Specifications)
                                                                    .Where(x => x.TypeId == ProductSpecificationType.Color.Id)
                                                    .Select(x => x.Value).FirstOrDefault();

                            var gender = product?.Variations?.SelectMany(x => x.Specifications)
                                                                    .Where(x => x.TypeId == ProductSpecificationType.Gender.Id)
                                                    .Select(x => x.Value).FirstOrDefault();

                            var size = product?.Variations?.SelectMany(x => x.Specifications)
                                                                    .Where(x => x.TypeId == ProductSpecificationType.Size.Id)
                                                    .Select(x => x.Value).FirstOrDefault();

                            var instagram = new ProductInstagramDto
                            {
                                Id = productId.ToString(),
                                Name = product?.Name,
                                Description = product?.Description,
                                Availability = "in stock",
                                BasePrice = variation?.BasePrice.ToString() + " BRL",
                                // Condition = ,
                                // Link = ,
                                Image_link = variation?.ImageUrl,
                                BrandName = product?.BrandName,
                                /*
                                Additional_image_link = ,
                                Age_group = ,
                                */
                                Color = color,
                                Gender = gender,
                                //Item_group_id = ,
                                /*
                                Fb_product_category = ,
                                Product_type = ,
                                Sale_price = ,
                                Sale_price_effective_date = ,
                                */
                                Size = size,
                                // Visibility = ,
                                // Inventory = ,
                                // Material = ,

                            };
                            instagramList.Add(instagram);
                        }
                    }
                }
                /*
                var skus = variations
                            .SelectMany(x => x)
                            .Select(x => x.StockKeepingUnit);   

                var stocks = (List<Stock>) this._context
                                .Stock
                                .Where(x => catalog.StoreCode == x.StoreCode
                                            && skus.Contains(x.StockKeepingUnit))
                                .Select(x => x);
                */
            }
            catch
            {
                res.IsError = true;
                res.Message = "Lista de produtos com itens inválidos.";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(500, res.Message));
                return res;
            }

            if (instagramList.Any())
            {
                res.Message = "Lista do Instagram";
                res.Payload = instagramList;
                return res;
            }
            else
            {
                res.IsError = true;
                res.Message = "Lista não encontrada";
                res.Errors = new List<Error>();
                res.Errors.Add(new Error(404, res.Message));
                return res;
            }
        }
    }
}