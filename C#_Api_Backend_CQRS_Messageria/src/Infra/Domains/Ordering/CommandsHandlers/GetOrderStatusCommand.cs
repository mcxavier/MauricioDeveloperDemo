using MediatR;
using Core.SharedKernel;
using Core.Domains.Ordering.Models;

namespace Infra.QueryCommands.Commands.Orders
{
    public class GetOrderStatusCommand : IRequest<Response>
    {
        public Order Order { get; set; }
    }
}
