using Core.Domains.Ordering.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Domains.Ordering.Repositories
{
    public interface IOrderRepository
    {
        bool CheckIfExistsOrders(int[] ids);
        Task<IList<Order>> GetOrdersAsync();
        bool ChangeOrderStatus(int id, int statusId);
    }
}