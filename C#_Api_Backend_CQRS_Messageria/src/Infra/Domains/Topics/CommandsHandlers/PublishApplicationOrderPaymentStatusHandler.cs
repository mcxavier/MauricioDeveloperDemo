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
    class PublishApplicationOrderPaymentStatusHandler : IRequestHandler<PublishApplicationOrderPaymentStatus, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationOrderPaymentStatusHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationOrderPaymentStatusHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationOrderPaymentStatusHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationOrderPaymentStatus request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.Where(x => x.Id.ToString() == request.OrderId).FirstOrDefault();

            if (order != null)
            {
                order.ModifiedBy = "Linx.IO";
                order.ModifiedAt = request.UpdatedAt;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return new Response("Notificação Pagamento do Pedido processda com sucesso.", false);
            }
            else
            {
                return new Response("Notificação Pagamento descartada porque o pedido não foi encontrado.", true);
            }
        }
    }
}
