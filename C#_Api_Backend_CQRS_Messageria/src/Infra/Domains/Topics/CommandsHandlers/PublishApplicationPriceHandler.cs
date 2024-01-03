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
using Utils;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationPriceHandler : IRequestHandler<PublishApplicationPrice, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationPriceHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly IProductCache _productCache;

        public PublishApplicationPriceHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationPriceHandler> logger,
            IProductCache productCache)
        {
            this._context = context;
            this._identity = identity;
            this._logger = logger;
            this._productCache = productCache;
        }

        public async Task<Response> Handle(PublishApplicationPrice request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Sku == null || request.SkuId == null)
                    return new Response("Atualização de preço descartada porque esta com Produto sem Sku", true);

                var variation = _context.ProductVariations.Where(x => x.OriginId == request.SkuId).FirstOrDefault();
                if (variation == null)
                {
                    variation = new ProductVariation
                    {
                        OriginId = request.SkuId,
                        BasePrice = request.SalesPrice > 0 ? request.SalesPrice : request.ListPrice,
                        ListPrice = request.ListPrice,
                        StockKeepingUnit = request.Sku,
                        CreatedAt = DateTimeBrazil.Now,
                        ModifiedAt = request.UpdatedAt,
                        ModifiedBy = "Linx.IO",
                        CreatedBy = "Linx.IO"
                    };

                    await _context.ProductVariations.AddAsync(variation);
                    await _context.SaveChangesAsync();

                    if (variation?.ProductId != null)
                        this._productCache.RemoveProductById(_context, (int)variation.ProductId, true);

                    return new Response("Preço registrado com sucesso.", false);
                }
                else
                {
                    variation.BasePrice = request.SalesPrice > 0 ? request.SalesPrice : request.ListPrice;
                    variation.ListPrice = request.ListPrice;
                    variation.StockKeepingUnit = request.Sku;
                    variation.ModifiedAt = request.UpdatedAt;
                    variation.ModifiedBy = "Linx.IO";

                    _context.ProductVariations.Update(variation);
                    await _context.SaveChangesAsync();

                    if (variation?.ProductId != null)
                        this._productCache.RemoveProductById(_context, (int)variation.ProductId, true);

                    return new Response("Preço atualizado com sucesso.", false);
                }
            }
            catch (System.Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true);
            }
        }
    }
}
