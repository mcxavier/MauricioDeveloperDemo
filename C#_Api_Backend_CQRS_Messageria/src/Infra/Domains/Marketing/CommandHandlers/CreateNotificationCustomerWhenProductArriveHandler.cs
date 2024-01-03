using System.Threading;
using System.Threading.Tasks;
using Core.Domains.Marketing.Commands.CreateNotificationCustomerWhenProductArrive;
using Core.Domains.Marketing.Models;
using Core.Domains.Marketing.Repositories;
using MediatR;

namespace Infra.Domains.Marketing.CommandHandlers
{

    public class CreateNotificationCustomerWhenProductArriveHandler : IRequestHandler<CreateNotificationCustomerWhenProductArriveCommand, CreateNotificationCustomerWhenProductArriveResponse>
    {
        private readonly IMarketingRepository _marketingRepository;

        public CreateNotificationCustomerWhenProductArriveHandler(IMarketingRepository marketingRepository)
        {
            _marketingRepository = marketingRepository;
        }

        public async Task<CreateNotificationCustomerWhenProductArriveResponse> Handle(CreateNotificationCustomerWhenProductArriveCommand request, CancellationToken cancellationToken)
        {
            var newNotify = new CustomerNotification(request.StockKeepingUnit, request.CustomerName, request.CustomerEmail);

            var success = await _marketingRepository.CreateNewCustomerNotification(newNotify);

            if (success)
            {
                return new CreateNotificationCustomerWhenProductArriveResponse
                {
                    Message = "Cadastrado com sucesso",
                    IsSuccess = true
                };
            }

            return new CreateNotificationCustomerWhenProductArriveResponse
            {
                Message = "Não foi possivel agendar o aviso, talvez você ja tenha selecionado este produto !!",
                IsSuccess = false
            };
        }
    }
}