using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Domains.Catalogs.Repositories;
using Core.Domains.Marketing.Commands.SendEmailToCustomerWhenProductArrive;
using Core.Domains.Marketing.DomainServices;
using Hangfire;
using Infra.ExternalServices.MailSender;
using Infra.ExternalServices.MailSender.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infra.Domains.Marketing.CommandHandlers
{

    public class SendEmailToCustomerWhenProductArriveHandler : IRequestHandler<SendEmailToCustomerWhenProductArriveCommand, SendEmailToCustomerWhenProductArriveResponse>
    {

        private readonly ICustomerNotificationEmailBuilder _customerNotificationEmailBuilder;
        private readonly IProductsRepository _productsRepository;
        private readonly IEmailServices _emailServices;
        private readonly IBackgroundJobClient _backgroundJobclient;
        private readonly ILogger<SendEmailToCustomerWhenProductArriveHandler> _logger;

        public SendEmailToCustomerWhenProductArriveHandler(IProductsRepository productsRepository, ICustomerNotificationEmailBuilder customerNotificationEmailBuilder, ILogger<SendEmailToCustomerWhenProductArriveHandler> logger, IEmailServices emailServices, IBackgroundJobClient backgroundJobclient)
        {
            this._customerNotificationEmailBuilder = customerNotificationEmailBuilder;
            this._productsRepository = productsRepository;
            this._emailServices = emailServices;
            this._backgroundJobclient = backgroundJobclient;
            this._logger = logger;
        }

        public async Task<SendEmailToCustomerWhenProductArriveResponse> Handle(SendEmailToCustomerWhenProductArriveCommand request, CancellationToken cancellationToken)
        {
            var variation = await _productsRepository.GetVariationByStockKeepingUnitAsync(request.StockKeepingUnit);

            var emailBody = _customerNotificationEmailBuilder.BuildEmailBody(request.CustomerName, variation);
            this._logger.LogDebug("mail body {@emailBody}", emailBody);


            var email = new EmailMessage
            {
                Subject = "O produto chegou !!",
                Body = emailBody,
                To = request.CustomerEmail,
                IsHtml = true,
                BCCs = new List<string>{
                    "jean.prates@linx.com.br",
                    "joao.martins@terceiroslinx.com.br",
                }
            };

            this._backgroundJobclient.Enqueue(() => this._emailServices.SendEmailMessage(email));

            return new SendEmailToCustomerWhenProductArriveResponse
            {
                IsSuccess = true,
                Message = "Command created"
            };
        }
    }
}