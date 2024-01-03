using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Ordering.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;
using Core.SharedKernel;
using Infra.ExternalServices.Payments.Dtos;
using Infra.ExternalServices.Payments.Vendors;

namespace Infra.ExternalServices.Payments.Contracts
{
    public interface IPaymentGatewayServices
    {
        GatewayProvider GetGatewayProvider();
        Task<KeyValuePair<string, ServiceResponse<PaymentDto>>> CreateOrder(Order order, List<OrderItem> items, OrderShipping shipping, PaymentCardDto paymentCardDto, Customer customer, PaymentTypeEnum paymentTypeId);
        Task<ServiceResponse<PaymentDto>> GetOrderStatus(string orderCode);
    }
}