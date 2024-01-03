using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.QuerysCommands.Commands.Orders.ChangeOrderStatus;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.QueryCommands.Commands.Orders
{
    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusCommand, ChangeOrderStatusResponse>
    {

        private readonly CoreContext _context;

        public ChangeOrderStatusHandler(CoreContext context)
        {
            this._context = context;
        }

        public async Task<ChangeOrderStatusResponse> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Where(order => request.OrderIds.Contains(order.Id)).ToListAsync(cancellationToken: cancellationToken);

            var mapped = orders.Select(order =>
            {
                order.StatusId = request.NewStatus;
                return order;
            });

            this._context.UpdateRange(mapped);
            var rows = await this._context.SaveChangesAsync();

            return new ChangeOrderStatusResponse(rows > 0, request.OrderIds, request.NewStatus);
        }
    }
}