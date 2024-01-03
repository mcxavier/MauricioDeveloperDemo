using Core.Models.Core.Customers;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infra.Domains.Topics.CommandsHandlers
{
    class PublishApplicationOrderTreatmentHandler : IRequestHandler<PublishApplicationOrderTreatment, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationOrderTreatmentHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationOrderTreatmentHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationOrderTreatmentHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationOrderTreatment request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _context.Orders.Where(x => x.Id.ToString() == request.OrderId).FirstOrDefault();
                if (order != null)
                {
                    if (order.ModifiedAt < request.UpdatedAt)
                    {
                        order.ModifiedBy = "Linx.IO";
                        order.ModifiedAt = request.UpdatedAt;
                        await _context.Orders.AddAsync(order);
                        await _context.SaveChangesAsync();

                        return new Response("Tratamento do Pedido processdo com sucesso.", false);
                    }
                    else
                    {
                        return new Response("Tratamento do Pedido descartado porque contem informação obsoleta.", true);
                    }
                }
                else
                {
                    return new Response("Tratamento do Pedido descartado porque o pedido não foi encontrado.", true);
                }

            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true);
            }
        }
    }
}
