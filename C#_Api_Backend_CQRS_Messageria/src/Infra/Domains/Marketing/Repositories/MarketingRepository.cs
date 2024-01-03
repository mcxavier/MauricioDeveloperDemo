using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domains.Marketing.Models;
using Core.Domains.Marketing.Repositories;
using Infra.EntitityConfigurations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{

    public class MarketingRepository : IMarketingRepository
    {
        private readonly CoreContext _context;

        public MarketingRepository(CoreContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNewCustomerNotification(CustomerNotification customerNotification)
        {
            var existing = this._context.CustomerNotifications.FirstOrDefault(x => x.CustomerEmail == customerNotification.CustomerEmail && x.StockKeepingUnit == customerNotification.StockKeepingUnit);
            if (existing != null)
            {
                return false;
            }

            await this._context.CustomerNotifications.AddAsync(customerNotification);
            var success = await this._context.SaveChangesAsync() > 0;

            return success;
        }

        public async Task<IList<CustomerNotification>> GetAllUnnotifiedCustomers(string storeCode)
        {
            var customerNotifications = await this._context.CustomerNotifications.Where(x => x.Notified == false && x.StoreCode == storeCode).ToListAsync();
            return customerNotifications;
        }
    }
}