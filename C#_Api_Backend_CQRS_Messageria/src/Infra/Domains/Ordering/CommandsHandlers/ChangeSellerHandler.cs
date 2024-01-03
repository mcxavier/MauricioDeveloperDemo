using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.QuerysCommands.Commands.Orders.ChangeSeller;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.QueryCommands.Commands.Orders
{
    public class ChangeSellerHandler : IRequestHandler<ChangeSellerCommand, ChangeSellerResponse>
    {

        private readonly CoreContext _context;

        public ChangeSellerHandler(CoreContext context)
        {
            this._context = context;
        }

        public async Task<ChangeSellerResponse> Handle(ChangeSellerCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(order => request.OrderId == order.Id);

            var seller = await _context.Sellers.FirstOrDefaultAsync(seller => seller.Id == request.NewSellerId);

            if (order == null 
                || (seller == null && request.NewSellerId != null)) 
                return new ChangeSellerResponse(false, 0, null);

            order.SellerId = request.NewSellerId;
            order.Seller = seller;

            this._context.Update(order);
            var rows = await this._context.SaveChangesAsync();

            return new ChangeSellerResponse(rows > 0, request.OrderId, request.NewSellerId);
        }
    }
}