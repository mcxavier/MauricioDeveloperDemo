using Infra.EntitityConfigurations.Contexts;
using Infra.ExternalServices.Authentication;
using Core.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Models.Core.Ordering;
using Core.Repositories;
using PagarMeApi;
using System;
using Utils;
using Newtonsoft.Json;

namespace Infra.Domains.Topics.CommandsHandlers
{
    class PublishApplicationOrderExceptionHandler : IRequestHandler<PublishApplicationOrderException, Response>
    {
        private readonly CoreContext _context;
        private readonly SmartSalesIdentity _identity;
        private readonly ICompanyRepository _companyRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ILogger<PublishApplicationOrderExceptionHandler> _logger;
        private LinxIOPagarMeClientConfig pagarMeConfig;

        public PublishApplicationOrderExceptionHandler(SmartSalesIdentity identity, ICompanyRepository companyRepository, IStoreRepository storeRepository, CoreContext context, ILogger<PublishApplicationOrderExceptionHandler> logger)
        {
            _storeRepository = storeRepository;
            this._companyRepository = companyRepository;
            _context = context;
            _identity = identity;
            _logger = logger;

            var config = _companyRepository.GetCompanySettingsAsync((this._identity.CurrentCompany ?? Guid.Empty), Core.Models.Identity.Companies.CompanySettingsType.PagarMeClientConfig).Result;
            this.pagarMeConfig = JsonConvert.DeserializeObject<LinxIOPagarMeClientConfig>(config.Value, JsonSettings.Settings);
        }

        public async Task<Response> Handle(PublishApplicationOrderException request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.Where(x => x.Id.ToString() == request.orderId).FirstOrDefault();

            if (order != null)
            {
                if (order.StatusId == OrderStatus.Cancelled.Id)
                {
                    return new Response("Exceção do pedido não processada porque o pedido já está cancelado.", true);
                }
                else if (order.StatusId == OrderStatus.Completed.Id)
                {
                    return new Response("Exceção do pedido não processada porque o pedido já está encerrado.", true);
                }
                else
                {
                    if (request.Type.Equals("CANCELLATION"))
                    {
                        order.StatusId = OrderStatus.Cancelled.Id;
                        order.ModifiedBy = "Linx.IO";
                        order.ModifiedAt = request.UpdatedAt;
                        await _context.SaveChangesAsync();

                        if ((request.ReturnProcessed == false) && (request.Refund.refundType == "PAYMENT_VOID") && (order.Payment.Transactions != null))
                        {
                            var store = await _storeRepository.GetStoreGatewaySettingsAsync(_identity.CurrentStoreId ?? Guid.Empty);

                            try
                            {
                                var client = new PagarmeCoreApiClient(this.pagarMeConfig.BasicAuthUserName, this.pagarMeConfig.BasicAuthPassword);
                                var PagarMeResponse = client.Charges.CancelCharge(order.OrderCode);
                            }
                            catch (Exception ex)
                            {
                                return new Response("Não possível extornar o pagamento: " + ex.Message, true);
                            }
                        }

                        return new Response("Cancelamento do pedido realizado com sucesso.", false);
                    }
                    else
                    {
                        return new Response("Exceção descartada porque não é um Cancelamento.", true);
                    }
                }
            }
            else
            {
                return new Response("Exceção descartada porque o pedido não foi encontrado.", true);
            }
        }
    }
}

