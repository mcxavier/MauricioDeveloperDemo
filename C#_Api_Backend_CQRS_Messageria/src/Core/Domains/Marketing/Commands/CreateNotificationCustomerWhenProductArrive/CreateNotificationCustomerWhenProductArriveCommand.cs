using MediatR;

namespace Core.Domains.Marketing.Commands.CreateNotificationCustomerWhenProductArrive
{
    public class CreateNotificationCustomerWhenProductArriveCommand : IRequest<CreateNotificationCustomerWhenProductArriveResponse>
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string StockKeepingUnit { get; set; }
    }
}