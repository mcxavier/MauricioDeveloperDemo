using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Marketing.Models;

namespace Core.Domains.Marketing.Repositories
{
    public interface IMarketingRepository
    {
        public Task<bool> CreateNewCustomerNotification(CustomerNotification customerNotification);
        public Task<IList<CustomerNotification>> GetAllUnnotifiedCustomers(string storeCode);
    }
}