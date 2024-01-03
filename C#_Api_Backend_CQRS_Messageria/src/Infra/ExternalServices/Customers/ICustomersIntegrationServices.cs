using System.Collections.Generic;
using System.Threading.Tasks;

using Infra.ExternalServices.Customers.Dtos;
using Infra.ExternalServices.Stock.Dtos;

namespace Infra.ExternalServices.Customers
{

    public interface ICustomersIntegrationServices
    {

        public Task<IList<UxCustomerDto>> GetAllCustomersAsync(string storeCode);

        public Task<IList<UxOrderHistoryDto>> GetOrderHistoryAsync(string storeCode);

    }

}