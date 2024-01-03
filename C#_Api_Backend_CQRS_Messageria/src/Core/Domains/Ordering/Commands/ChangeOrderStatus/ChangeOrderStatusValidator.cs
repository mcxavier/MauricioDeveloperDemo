using Core.Domains.Ordering.Repositories;
using Core.Models.Core.Ordering;
using FluentValidation;

namespace Core.QuerysCommands.Commands.Orders.ChangeOrderStatus
{

    public class ChangeOrderStatusValidator : AbstractValidator<ChangeOrderStatusCommand>
    {

        private readonly IOrderRepository _orderRepository;

        public ChangeOrderStatusValidator(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;

            RuleFor(x => x.NewStatus).Must(CheckStatus).WithMessage("Novo status Inválido");

            RuleFor(x => x.OrderIds).Must(ExistsOrders).WithMessage("Um ou mais pedidos não existem ou são inválidos");
        }

        private bool CheckStatus(int id) =>
            (OrderStatusEnum)id switch
            {
                OrderStatusEnum.Submitted => true,
                OrderStatusEnum.AwaitingValidation => true,
                OrderStatusEnum.StockConfirmed => true,
                OrderStatusEnum.Paid => true,
                OrderStatusEnum.Shipped => true,
                OrderStatusEnum.Cancelled => true,
                OrderStatusEnum.PaymentFailed => true,
                OrderStatusEnum.Completed => true,
                _ => false
            };

        private bool ExistsOrders(int[] ids)
        {
            return this._orderRepository.CheckIfExistsOrders(ids);
        }
    }
}