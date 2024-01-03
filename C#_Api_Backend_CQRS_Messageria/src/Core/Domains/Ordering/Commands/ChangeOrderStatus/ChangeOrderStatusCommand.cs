using MediatR;

namespace Core.QuerysCommands.Commands.Orders.ChangeOrderStatus
{
    public class ChangeOrderStatusCommand : IRequest<ChangeOrderStatusResponse>
    {
        public int[] OrderIds { get; set; }
        public int NewStatus { get; set; }
    }
}