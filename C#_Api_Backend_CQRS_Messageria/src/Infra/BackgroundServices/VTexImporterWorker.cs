using Core.Enums;
using CoreService.IntegrationsViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using SqlBulkTools;
using SqlBulkTools.Enumeration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Models.Core.Products;
using Core.Repositories;
using Hangfire.Server;
using Infra.BackgroundServices.Configurations;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Catalog;
using Serilog;
using Utils.Extensions;

namespace Api.BackgroundServices
{

    public class VTexImporterWorker
    {
        private readonly IProductIntegrationService _productsIntegrationService;
        private readonly IServiceProvider _services;
        private ILogger _logger { get; set; }
        private const int PerPage = 25;

        public VTexImporterWorker(IProductIntegrationService productsIntegration, IServiceProvider services)
        {
            this._productsIntegrationService = productsIntegration;
            this._services = services;
        }


        public void TaskMethod(PerformContext context = null)
        {
            this._logger = context.CreateLoggerForPerformContext<VTexImporterWorker>();
            this._logger.Information("Importador executando ...");

            try
            {
                using (var scope = _services.CreateScope())
                {
                    var tenantQueries = scope.ServiceProvider.GetService<ITenantRepository>();
                    var tenants = tenantQueries.GetAllTenants().Result;

                    foreach (var tenant in tenants)
                    {
                        var identity = scope.ServiceProvider.GetService<SmartSalesIdentity>();
                        identity.Name = "System";
                        identity.CurrentCompany = tenant.CompanyId;

                        var logger = scope.ServiceProvider.GetService<Microsoft.Extensions.Logging.ILogger<CoreContext>>();
                        var coreContext = scope.ServiceProvider.GetService<CoreContext>();

                        var vtexIdsResponse = _productsIntegrationService.GetProductIds().Result;
                        var miss = vtexIdsResponse.Range.Total;

                        for (var current = 0; current <= miss; current += PerPage)
                        {
                            StartWorkAsync(coreContext, current, miss).Wait();
                            miss -= PerPage;

                            this._logger.Information("vtext Importer Worker esperando 1s");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this._logger.Error("erro em vtext Importer Worker from {time} \n {@exception}", DateTimeOffset.Now, exception);
                throw;
            }
        }

        protected async Task StartWorkAsync(CoreContext context, int currentPage, int miss)
        {
            await ImportProductsPage(context, currentPage, currentPage + PerPage);
        }


        private async Task ImportProductsPage(CoreContext context, int currentPage, int perPage)
        {
            var watchTotal = new Stopwatch();
            watchTotal.Start();

            var watch = new Stopwatch();
            watch.Start();

            var ids = await _productsIntegrationService.GetProductIds(currentPage, perPage);

            var getProductsById = ids.Data.Keys.Select(key => _productsIntegrationService.GetById(key.ToInt()));
            var productResult = await Task.WhenAll(getProductsById);
            Thread.Sleep(1000);
            var variationIds = productResult.SelectMany(product => ids.Data[product.Id.ToString()]);
            var getVariations = variationIds.Select(variation => _productsIntegrationService.GetProductVariationsById(variation));
            var getPrices = variationIds.Select(variation => _productsIntegrationService.GetProductVariationPrices(variation));
            var variations = await Task.WhenAll(getVariations);
            Thread.Sleep(1000);

            var prices = await Task.WhenAll(getPrices);
            Thread.Sleep(1000);

            _logger.Information("Request VTEX {ellapsed}", watch.Elapsed);
            watch.Reset();
            watch.Start();

            var productsReferencesLookup = productResult
                                       .ToDictionary(p => p.Id.Value, p => p.KeyWords.TrySplit(",")[0]);

            await InsertProducts(productResult, context);

            _logger.Information("Insert products {ellapsed}", watch.Elapsed);
            watch.Reset();
            watch.Start();

            var productsIds = variations.Select(variation => productsReferencesLookup[variation.ProductId]);
            var productsLookup = context.Products.Where(product => productsIds.Contains(product.CommonReference))
                                                 .ToDictionary(product => product.CommonReference, product => product.Id);

            await InsertVariations(variations, prices, productsLookup, productsReferencesLookup, context);

            _logger.Information("Insert variations {ellapsed}", watch.Elapsed);
            watch.Reset();
            watch.Start();

            var variationsIds = variations.Select(variation => variation.Id.ToString()).ToList();
            var variationslookup = context.ProductVariations.Where(variation => variationsIds.Contains(variation.OriginId))
                                                            .ToDictionary(variation => variation.OriginId, variation => variation.Id);

            await InsertSpecifications(variations, variationslookup, context);

            _logger.Information("Insert Specifications {ellapsed}", watch.Elapsed);
            watch.Reset();
            watch.Start();

            await InsertImages(variations, variationslookup, context);

            _logger.Information("Ellapsed images {ellapsed}", watch.Elapsed);
            watch.Reset();
            watch.Start();

            await InsertCategories(variations, context);
            var categoriesLookup = context.Categories.ToDictionary(category => category.Name, category => category.Id);
            await InsertProductCategories(variations, productsLookup, productsReferencesLookup, categoriesLookup, context);

            _logger.Information("Insert Categories {ellapsed}", watch.Elapsed);
            watch.Stop();

            _logger.Information("########################################## TOTAL ############################# {ellapsed}", watchTotal.Elapsed);
            watchTotal.Stop();
        }

        private async Task InsertProducts(IEnumerable<VtexProduct> products, CoreContext context)
        {
            var bulk = new BulkOperations();
            var entityProducts = products
                                    .GroupBy(x => new { CommonReference = x.KeyWords.TrySplit(",")[0] })
                                    .Select(x =>
                                    {
                                        var product = x.FirstOrDefault(y => x.Min(p => p.Id) == y.Id);

                                        return new Product
                                        {
                                            Ncm = product.TaxCode,
                                            CommonReference = x.Key.CommonReference,
                                            OriginId = x.Min(p => p.Id).ToString(),
                                            Origin = $"Importation - Vtex {DateTime.Now:g} - {Environment.MachineName}",
                                            Reference = product.RefId,
                                            Name = product.Name,
                                            Description = product.Description,
                                            BrandName = product.BrandName,
                                            CreatedAt = DateTime.Now,
                                            IsActive = product.IsActive,
                                        };
                                    });

            using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                bulk.Setup<Product>()
                        .ForCollection(entityProducts)
                        .WithTable("product.Products")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .ExcludeColumnFromUpdate(x => x.IsActive)
                        .MatchTargetOn(x => x.CommonReference)
                        .Commit(connection);
            }
        }

        private async Task InsertVariations(IEnumerable<VtexProductVariation> variations,
                                            IEnumerable<VtexBasePrices> prices,
                                            IDictionary<string, int> productsLookup,
                                            IDictionary<int, string> productsReferencesLookup,
                                            CoreContext context)
        {

            var bulk = new BulkOperations();
            var variationsDto = variations.Select(v =>
            {
                var price = prices.FirstOrDefault(x => x?.ItemId?.ToInt() == v.Id);

                return new ProductVariation()
                {
                    Name = v.ProductName,
                    CompleteName = v.SkuName,
                    BasePrice = price?.BasePrice ?? (decimal)0,
                    CostPrice = price?.CostPrice ?? (decimal)0,
                    ListPrice = price?.ListPrice ?? (decimal)0,
                    CreatedAt = DateTime.Now,
                    StockKeepingUnit = (v.AlternateIds?.RefId ?? "").Replace("/", "").Replace("-", "") ?? "UNKNOWN",
                    OriginId = v.Id.ToString(),
                    IsActive = v.IsActive,
                    Origin = $"IMPORTATION - VTEX {DateTime.Now:g} - {Environment.MachineName}",
                    ProductId = productsLookup[productsReferencesLookup[v.ProductId]]
                };
            }).ToList();

            using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                bulk.Setup<ProductVariation>()
                        .ForCollection(variationsDto)
                        .WithTable("product.Variations")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .ExcludeColumnFromUpdate(x => x.IsActive)
                        .MatchTargetOn(x => x.OriginId)
                        .Commit(connection);
            }
        }

        private async Task InsertSpecifications(IEnumerable<VtexProductVariation> variations,
                                                IDictionary<string, int> variationsLookup,
                                                CoreContext context)
        {
            var bulk = new BulkOperations();

            var specifications = variations.SelectMany(
                (variations) => variations.SkuSpecifications.Concat(variations.ProductSpecifications),
                (variation, specification) =>
                {
                    var spec = GetSpecificationType(specification);

                    return new ProductSpecification
                    {
                        Name = spec.Name.Truncate(25),
                        Description = spec.Description.Truncate(25),
                        Value = spec.Value.Truncate(25),
                        IsFilter = spec.IsFilter,
                        TypeId = spec.TypeId,
                        ProductVariationId = variationsLookup[variation.Id.ToString()],
                    };
                });

            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                bulk.Setup<ProductSpecification>()
                        .ForCollection(specifications)
                        .WithTable("product.Specifications")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(x => x.ProductVariationId)
                        .MatchTargetOn(x => x.TypeId)
                        .MatchTargetOn(x => x.Value)
                        .Commit(connection);
            }
        }

        private async Task InsertImages(IEnumerable<VtexProductVariation> variations,
                                        IDictionary<string, int> variationsLookup,
                                        CoreContext context)
        {

            var bulk = new BulkOperations();

            var images = variations
                .Select(x => new KeyValuePair<int, List<VtexProductVariationImage>>(x.Id, x.Images))
                .SelectMany(x => (x.Value), (keyvalue, image) => new ProductImage
                {
                    ProductVariationId = variationsLookup[keyvalue.Key.ToString()],
                    Name = image.ImageName,
                    UrlImage = image.ImageUrl
                });

            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                bulk.Setup<ProductImage>()
                        .ForCollection(images)
                        .WithTable("product.Images")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(x => x.ProductVariationId)
                        .MatchTargetOn(x => x.UrlImage)
                        .MatchTargetOn(x => x.Name)
                        .Commit(connection);
            }
        }

        private async Task InsertProductCategories(IEnumerable<VtexProductVariation> variations,
                                                   IDictionary<string, int> productsLookup,
                                                   IDictionary<int, string> productsReferencesLookup,
                                                   IDictionary<string, int> categoriesLookup,
                                                   CoreContext context)
        {

            var bulk = new BulkOperations();

            var productCategories = variations.SelectMany(
                                                 variation => variation.ProductCategories,
                                                 (variation, category) => new ProductCategory
                                                 {
                                                     ProductId = productsLookup[productsReferencesLookup[variation.ProductId]],
                                                     CategoryId = categoriesLookup[category.Value]
                                                 }
                                               );


            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                bulk.Setup<ProductCategory>()
                        .ForCollection(productCategories.DistinctBy(x => new { x.ProductId, x.CategoryId }))
                        .WithTable("product.ProductCategory")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(x => x.ProductId)
                        .MatchTargetOn(x => x.CategoryId)
                        .Commit(connection);
            }
        }

        private async Task InsertCategories(IEnumerable<VtexProductVariation> variations,
                                            CoreContext context)
        {

            var bulk = new BulkOperations();

            var categories = variations
                                       .SelectMany(x => x.ProductCategories,
                                            (variation, category) => new Category() { Name = category.Value, Description = category.Value });

            await using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
            {

                await connection.OpenAsync();
                bulk.Setup<Category>()
                        .ForCollection(categories.DistinctBy(x => x.Name))
                        .WithTable("product.Categories")
                        .AddAllColumns()
                        .BulkInsertOrUpdate()
                        .SetIdentityColumn(x => x.Id, ColumnDirectionType.InputOutput)
                        .MatchTargetOn(x => x.Name)
                        .Commit(connection);
            }
        }

        private ProductSpecification GetSpecificationType(VtexProductVariationSpecification specification)
        {
            if (GetFieldTypeMapped(specification.FieldName).Id == ProductSpecificationType.Color.Id)
            {
                return new ProductSpecification
                {
                    Name = specification.FieldName,
                    Description = specification.FieldValues.FirstOrDefault(),
                    Value = specification.FieldValues.FirstOrDefault(),
                    TypeId = this.GetFieldTypeMapped(specification.FieldName).Id,
                    IsFilter = specification.IsFilter,
                };
            }

            return new ProductSpecification
            {
                Name = specification.FieldName,
                Description = specification.FieldValues.FirstOrDefault(),
                Value = ChooseSizesValues(specification.FieldValues.FirstOrDefault()),
                TypeId = this.GetFieldTypeMapped(specification.FieldName).Id,
                IsFilter = specification.IsFilter,
            };
        }

        private ProductSpecificationType GetFieldTypeMapped(string vtexFildName) => vtexFildName switch
        {
            "Tamanho" => ProductSpecificationType.Size,
            "Cor" => ProductSpecificationType.Color,
            "Gênero" => ProductSpecificationType.Gender,
            "Cor Simples" => ProductSpecificationType.SimpleColor,
            _ => ProductSpecificationType.Others
        };

        private string ChooseSizesValues(string vtexFildName) => vtexFildName switch
        {
            "UN" => ((int)SizesEnum.UN).ToString(),
            "P" => ((int)SizesEnum.P).ToString(),
            "M" => ((int)SizesEnum.M).ToString(),
            "G" => ((int)SizesEnum.G).ToString(),
            "GG" => ((int)SizesEnum.GG).ToString(),
            "XGG" => ((int)SizesEnum.XGG).ToString(),
            "XXG" => ((int)SizesEnum.XXG).ToString(),
            _ => vtexFildName
        };

        private bool IgnoreImages(VtexProductVariationImage img)
        {
            return !img.ImageUrl.Contains("Produto-sem-Imagem") &&
                   !img.ImageUrl.Contains("PRODUTOSEMFOTO");

        }
    }
}