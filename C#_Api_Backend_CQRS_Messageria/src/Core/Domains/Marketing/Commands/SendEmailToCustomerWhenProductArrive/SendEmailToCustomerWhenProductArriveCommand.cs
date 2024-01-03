using MediatR;

namespace Core.Domains.Marketing.Commands.SendEmailToCustomerWhenProductArrive
{

    public class SendEmailToCustomerWhenProductArriveCommand : IRequest<SendEmailToCustomerWhenProductArriveResponse>
    {
        public SendEmailToCustomerWhenProductArriveCommand(string stockKeepingUnit, string customerName, string customerEmail)
        {
            this.StockKeepingUnit = stockKeepingUnit;
            this.CustomerName = customerName;
            this.CustomerEmail = customerEmail;
        }

        public string StockKeepingUnit { get; private set; }
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; private set; }
    }
}