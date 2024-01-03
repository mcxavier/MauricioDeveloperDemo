using Core.Domains.Ordering.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IOrdersRepository
    {
        Task<IList<Order>> GetOrdersAsync(Guid companyId);
    }
}
