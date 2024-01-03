using Core.Models.Core.Customers;
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
    class PublishApplicationSellersHandler : IRequestHandler<PublishApplicationSellers, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationSellersHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationSellersHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationSellersHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationSellers request, CancellationToken cancellationToken)
        {
            try
            {
                var seller = _context.Sellers.FirstOrDefault(x => x.OriginId == request.OriginId);
                if (seller == null)
                {
                    seller = new Seller
                    {
                        CreatedAt = DateTimeBrazil.Now,
                        ModifiedAt = request.ModifiedAt,
                        CreatedBy = request.CreatedBy,
                        ModifiedBy = request.ModifiedBy,
                        IsActive = request.IsActive,
                        Name = request.Name,
                        Phone = request.Phone,
                        Email = request.Email,
                        Document = request.Document,
                        BirthDay = request.BirthDay,
                        OriginId = request.OriginId
                    };

                    await _context.Sellers.AddAsync(seller);
                    await _context.SaveChangesAsync();

                    return new Response("Vendedor cadastrado com sucesso", false);
                }
                else
                {
                    seller.Name = request.Name;
                    seller.Phone = request.Phone;
                    seller.Email = request.Email;
                    seller.Document = request.Document;
                    seller.BirthDay = request.BirthDay;
                    seller.IsActive = request.IsActive;
                    seller.ModifiedAt = request.ModifiedAt;
                    seller.ModifiedBy = request.ModifiedBy;

                    _context.Sellers.Update(seller);
                    await _context.SaveChangesAsync();

                    return new Response("Dados do Vendedor atualizados com sucesso", false);
                }
            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true, request);
            }
        }
    }
}
