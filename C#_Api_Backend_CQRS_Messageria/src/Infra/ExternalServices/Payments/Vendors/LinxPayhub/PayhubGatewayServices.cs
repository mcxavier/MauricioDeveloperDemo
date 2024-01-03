using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.Repositories;
using Core.SharedKernel;
using Infra.ExternalServices.Authentication;
using Infra.ExternalServices.Payments.Contracts;
using Infra.ExternalServices.Payments.Dtos;
using Microsoft.Extensions.Logging;

namespace Infra.ExternalServices.Payments.Vendors.LinxPayhub
{

    public class PayhubGatewayServices : IPaymentGatewayServices
    {
        public GatewayProvider GetGatewayProvider() => GatewayProvider.LinxPayhub;

        private readonly IPaymentGatewayServices _gatewayServices;
        private readonly ILogger<PayhubGatewayServices> _logger;
        private readonly IStoreRepository _storeRepository;
        private readonly SmartSalesIdentity _identity;

        public PayhubGatewayServices(IPaymentGatewayServices gatewayServices,
                                     ILogger<PayhubGatewayServices> logger,
                                     IStoreRepository storeRepository,
                                     SmartSalesIdentity identity)
        {

            this._gatewayServices = gatewayServices;
            this._storeRepository = storeRepository;
            this._identity = identity;
            this._logger = logger;
        }

        public async Task<ServiceResponse<PaymentDto>> GetOrderStatus(string orderCode)
        {
            return await this._gatewayServices.GetOrderStatus(orderCode);
        }

        public async Task<KeyValuePair<string, ServiceResponse<PaymentDto>>> CreateOrder(Order order, List<OrderItem> items, OrderShipping shipping, PaymentCardDto paymentCardDto, Customer customer, PaymentTypeEnum paymentTypeId)
        {
            return await this._gatewayServices.CreateOrder(order, items, shipping, paymentCardDto, customer, paymentTypeId);
        }
    }
}