using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Infra.Extensions;
using System.Linq;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationStockHandler : IRequestHandler<PublishApplicationStock, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationStockHandler> _logger;
        private readonly SmartSalesIdentity _identity;
        private readonly IProductCache _productCache;

        public PublishApplicationStockHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationStockHandler> logger, IProductCache productCache)
        {
            this._context = context;
            this._identity = identity;
            this._logger = logger;
            this._productCache = productCache;
        }

        public async Task<Response> Handle(PublishApplicationStock request, CancellationToken cancellationToken)
        {
            try
            {
                var stock = _context.Stock.FirstOrDefault(x => x.StockKeepingUnit == request.Sku && x.StoreCode == request.LocationId);
                if (stock == null)
                {
                    stock = new Stock
                    {
                        StockKeepingUnit = request.Sku,
                        StoreCode = request.LocationId,
                        LastSinc = request.UpdatedAt,
                        Units = request.AvailableQuantity
                    };

                    await _context.Stock.AddAsync(stock);
                    await _context.SaveChangesAsync();

                    var variation = _context.ProductVariations.FirstOrDefault(x => x.StockKeepingUnit == stock.StockKeepingUnit);

                    if (variation?.ProductId != null)
                        this._productCache.RemoveProductById(_context, (int)variation.ProductId, true);

                    return new Response("Estoque cadastrado com sucesso.", false);
                }
                else
                {
                    if (stock.LastSinc < request.UpdatedAt)
                    {
                        stock.Units = request.AvailableQuantity;
                        stock.LastSinc = request.UpdatedAt;
                        _context.Stock.Update(stock);
                        await _context.SaveChangesAsync();

                        var variation = _context.ProductVariations.FirstOrDefault(x => x.StockKeepingUnit == stock.StockKeepingUnit);
                        if (variation?.ProductId != null)
                            this._productCache.RemoveProductById(_context, (int)variation.ProductId, true);

                        return new Response("Estoque atualizado com sucesso.", false);
                    }
                    else
                    {
                        return new Response("Registro de estoque descartado porque contém informação obsoleta.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true, request);
            }
        }
    }
}
