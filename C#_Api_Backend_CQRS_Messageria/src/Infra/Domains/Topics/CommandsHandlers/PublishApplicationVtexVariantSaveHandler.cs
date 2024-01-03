using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Core.Models.Core.Products;

namespace Infra.Domains.Topics.CommandsHandlers
{
    public class PublishApplicationVtexVariantSaveHandler : IRequestHandler<PublishApplicationVtexVariantSave, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationVtexVariantSaveHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationVtexVariantSaveHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationVtexVariantSaveHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationVtexVariantSave request, CancellationToken cancellationToken)
        {

            try
            {
                int iId = 0;
                Int32.TryParse(request.productVariationId, out iId);

                if (request != null)
                {
                    var productVariation = _context.ProductVariations.Where(x => x.Id == iId).FirstOrDefault();
                    if (productVariation != null)
                    {
                        var productImage = _context.ProductImages.Where(x => x.Id == productVariation.Id && x.Name == request.name).FirstOrDefault();
                        if (productImage == null)
                        {
                            productImage = new ProductImage()
                            {
                                Name = request.name,
                                UrlImage = request.urlImage,
                                IsPrincipal = request.isPrincipal,
                                ProductVariationId = productVariation.Id,
                                ProductVariation = productVariation
                            };

                            _context.ProductImages.Add(productImage);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            productImage.UrlImage = request.urlImage;
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

                await _context.SaveChangesAsync();

                return new Response("Imagem salva com sucesso.", false);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, true);
            }
        }
    }
}
