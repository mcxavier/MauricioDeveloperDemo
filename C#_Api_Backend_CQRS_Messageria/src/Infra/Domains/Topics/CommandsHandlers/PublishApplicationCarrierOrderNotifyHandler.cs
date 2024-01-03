using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infra.Domains.Topics.CommandsHandlers
{
    class PublishApplicationCarrierOrderNotifyHandler : IRequestHandler<PublishApplicationCarrierOrderNotify, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationCarrierOrderNotifyHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationCarrierOrderNotifyHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationCarrierOrderNotifyHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationCarrierOrderNotify request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.Where(x => x.Id.ToString() == request.OrderId).FirstOrDefault();

            if (order != null)
            {
                order.ModifiedBy = "Linx.IO";
                order.ModifiedAt = request.UpdatedAt;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }

            return new Response("Notificação de Pedido processda.", false);
        }
    }
}

