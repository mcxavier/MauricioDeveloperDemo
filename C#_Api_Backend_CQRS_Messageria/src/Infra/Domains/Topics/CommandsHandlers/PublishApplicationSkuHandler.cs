using Core.Models.Core.Products;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Utils;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationSkuHandler : IRequestHandler<PublishApplicationSku, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationSkuHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationSkuHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationSkuHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationSku request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Sku == null || request.SkuId == null)
                    return new Response("Mensagem descartada porque contém Produto sem Sku. ", false);

                var variation = _context.ProductVariations.Where(x => x.OriginId == request.SkuId).FirstOrDefault();
                if (variation == null)
                {
                    variation = new ProductVariation
                    {
                        StockKeepingUnit = request.Sku,
                        OriginId = request.SkuId,
                        CreatedAt = DateTimeBrazil.Now,
                        CreatedBy = "Linx.IO",
                        ModifiedAt = request.UpdatedAt,
                        ModifiedBy = "Linx.IO"
                    };

                    await _context.ProductVariations.AddAsync(variation);
                    await _context.SaveChangesAsync();

                    return new Response("Sku cadastrado com sucesso.", false);
                }
                else
                {
                    variation.StockKeepingUnit = request.Sku;
                    variation.OriginId = request.SkuId;
                    variation.ModifiedAt = request.UpdatedAt;
                    variation.ModifiedBy = "Linx.IO";

                    _context.ProductVariations.Update(variation);
                    await _context.SaveChangesAsync();

                    return new Response("Sku atualizado com sucesso.", false);
                }
            }
            catch (Exception ex)
            {
                return new Response("ERRO:" + ex.Message, true);
            }
        }
    }
}
