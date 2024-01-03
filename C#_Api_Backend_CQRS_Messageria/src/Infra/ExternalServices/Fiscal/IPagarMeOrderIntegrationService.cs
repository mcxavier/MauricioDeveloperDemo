using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Domains.Ordering.Models;
using Core.Models;
using Core.Models.Core.Customers;
using Core.Models.Core.Ordering;
using Core.Models.Core.Payments;

namespace Infra.ExternalServices.Fiscal
{

    public interface IPagarMeOrderIntegrationService
    {

        Task<bool> CreateOrder(Order order, List<OrderItem> items, OrderShipping shipping, Payment paymentCard, Customer customer);

    }

}