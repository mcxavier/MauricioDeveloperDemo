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
using Utils.Extensions;

namespace Infra.QueryCommands.Commands.Topics
{
    class PublishApplicationOrderStatusHandler : IRequestHandler<PublishApplicationOrderStatus, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationOrderHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationOrderStatusHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationOrderHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationOrderStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var order = this._context.Orders.FirstOrDefault(x => x.Id == request.OrderId.ToInt());

                if (order != null)
                {
                    order.StatusId = request.Status.Id;
                    order.ModifiedBy = "Linx.IO";
                    order.ModifiedAt = request.UpdatedAt;
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                    return new Response("Status do pedido atualizado com sucesso", false, null);
                }
                else
                {
                    return new Response("Status de Order descartado porque o pedido não foi encontrado.", true, null);
                }
            }
            catch (Exception ex)
            {
                return new Response("ERRO: " + ex.Message, true, null);
            }
        }
    }
}
