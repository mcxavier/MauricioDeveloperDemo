using Core.Models.Core.Ordering;
using Core.SharedKernel;
using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Domains.Topics.CommandsHandlers
{
    public class PublishApplicationFeedBackHandler : IRequestHandler<PublishApplicationFeedBack, Response>
    {
        private readonly CoreContext _context;
        private readonly ILogger<PublishApplicationFeedBackHandler> _logger;
        private readonly SmartSalesIdentity _identity;

        public PublishApplicationFeedBackHandler(CoreContext context, SmartSalesIdentity identity, ILogger<PublishApplicationFeedBackHandler> logger)
        {
            _context = context;
            _identity = identity;
            _logger = logger;
        }

        public async Task<Response> Handle(PublishApplicationFeedBack request, CancellationToken cancellationToken)
        {
            if (request.topicId.ToUpper().Equals("ORDER"))
            {
                var order = _context.Orders.Where(x => x.Id.ToString() == request.entityId).FirstOrDefault();
                if (order != null)
                {
                    if (request.Type == "ERROR")
                    {
                        order.StatusId = OrderStatus.IntegrationFailed.Id;
                    }
                    else
                    {
                        order.StatusId = OrderStatus.IntegrationSuccess.Id;
                    }

                    order.ModifiedBy = "Linx.IO";
                    order.ModifiedAt = request.updatedAt;
                    await _context.SaveChangesAsync();

                    return new Response("FeedBack processado com suscesso.", false);
                }
                else
                {
                    return new Response("FeedBack descartado por ser relativo a um pedido inexistente.", true);
                }
            }
            else
            {
                return new Response("FeedBack descartado por não ser relativo a uma ORDER.", true);
            }
        }
    }
}

