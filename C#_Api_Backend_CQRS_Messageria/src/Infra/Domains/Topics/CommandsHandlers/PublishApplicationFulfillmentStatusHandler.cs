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
using Core.Models.Core.Ordering;

namespace Infra.Domains.Topics.CommandsHandlers
{
    class PublishApplicationFulfillmentStatusHandler : IRequestHandler<PublishApplicationFulfillmentStatus, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationFulfillmentStatusHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationFulfillmentStatusHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationFulfillmentStatusHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationFulfillmentStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _context.Orders.Where(x => x.Id.ToString() == request.orderId).FirstOrDefault();

                if (order != null)
                {
                    if (order.StatusId == OrderStatus.Cancelled.Id)
                    {
                        return new Response("Atualização do pedido não processada porque o pedido já está cancelado.", true);
                    }
                    else if (order.StatusId == OrderStatus.Completed.Id)
                    {
                        return new Response("Atualização do pedido não processada porque o pedido já está encerrado.", true);
                    }
                    else
                    {

                        if (request.status.Equals("PICKING"))
                        {
                            order.StatusId = OrderStatus.Shipped.Id;
                        }
                        else if (request.status.Equals("WAITING")) //
                        {
                            order.StatusId = OrderStatus.StockConfirmed.Id;
                        }
                        else if (request.status.Equals("DELIVERED"))
                        {
                            order.StatusId = OrderStatus.Completed.Id;
                        }
                        else if (request.status.Equals("INVOICED"))
                        {
                            order.StatusId = OrderStatus.Invoiced.Id;
                        }
                        else if (request.status.Equals("CANCELED"))
                        {
                            order.StatusId = OrderStatus.Cancelled.Id;
                        }
                        
                        order.ModifiedBy = "Linx.IO";
                        order.ModifiedAt = request.updatedAt;
                        await _context.SaveChangesAsync();

                        return new Response("Atualização de fullfiment processada.", false);
                    }
                }
                else
                {
                    return new Response("Atualização de fullfiment descartada porque o pedido não foi encontrado.", true);
                }

            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true);
            }

        }
    }
}
