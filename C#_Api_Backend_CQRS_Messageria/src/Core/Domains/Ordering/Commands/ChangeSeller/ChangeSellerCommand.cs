using MediatR;

namespace Core.QuerysCommands.Commands.Orders.ChangeSeller
{
    public class ChangeSellerCommand : IRequest<ChangeSellerResponse>
    {
        public int OrderId { get; set; }
        public int? NewSellerId { get; set; }
    }
}